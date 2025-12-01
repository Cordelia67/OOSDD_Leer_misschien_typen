using Domain.Models;
using Domain.Repositories;
using System.Text.RegularExpressions;

namespace Typotrainer.ViewModels
{
    /// <summary>
    /// Register ViewModel - handles registration logic with database
    /// </summary>
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;

        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _emailError = string.Empty;
        private string _passwordError = string.Empty;
        private string _generalError = string.Empty;
        private bool _isEmailErrorVisible = false;
        private bool _isPasswordErrorVisible = false;
        private bool _isGeneralErrorVisible = false;

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                ValidateEmailRealTime();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                ValidatePasswordRealTime();
            }
        }

        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value);
        }

        public string PasswordError
        {
            get => _passwordError;
            set => SetProperty(ref _passwordError, value);
        }

        public string GeneralError
        {
            get => _generalError;
            set => SetProperty(ref _generalError, value);
        }

        public bool IsEmailErrorVisible
        {
            get => _isEmailErrorVisible;
            set => SetProperty(ref _isEmailErrorVisible, value);
        }

        public bool IsPasswordErrorVisible
        {
            get => _isPasswordErrorVisible;
            set => SetProperty(ref _isPasswordErrorVisible, value);
        }

        public bool IsGeneralErrorVisible
        {
            get => _isGeneralErrorVisible;
            set => SetProperty(ref _isGeneralErrorVisible, value);
        }

        // Event voor succesvolle registratie
        public event EventHandler? RegistrationSuccessful;

        public RegisterViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private async void ValidateEmailRealTime()
        {
            IsEmailErrorVisible = false;
            IsGeneralErrorVisible = false;

            if (!string.IsNullOrEmpty(Email) && Email.Contains("@"))
            {
                if (!IsValidEmail(Email))
                {
                    EmailError = "Voer een geldig e-mailadres in";
                    IsEmailErrorVisible = true;
                    return;
                }

                // Check of email al bestaat
                if (await _userRepository.EmailExistsAsync(Email.ToLower()))
                {
                    EmailError = "Er bestaat al een account met dit e-mailadres";
                    IsEmailErrorVisible = true;
                }
            }
        }

        private void ValidatePasswordRealTime()
        {
            IsPasswordErrorVisible = false;
            IsGeneralErrorVisible = false;

            if (!string.IsNullOrEmpty(Password))
            {
                if (Password.Length < 4)
                {
                    PasswordError = "Wachtwoord moet minimaal 4 tekens bevatten";
                    IsPasswordErrorVisible = true;
                }
                else if (!IsValidPassword(Password))
                {
                    PasswordError = "Wachtwoord mag geen ongeldige tekens bevatten";
                    IsPasswordErrorVisible = true;
                }
            }
        }

        public async Task<bool> RegisterAsync()
        {
            // Verberg foutmeldingen
            IsEmailErrorVisible = false;
            IsPasswordErrorVisible = false;
            IsGeneralErrorVisible = false;

            // Validatie: velden ingevuld
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Voer een e-mailadres in";
                IsEmailErrorVisible = true;
                GeneralError = "Vul alle velden in";
                IsGeneralErrorVisible = true;
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Voer een wachtwoord in";
                IsPasswordErrorVisible = true;
                GeneralError = "Vul alle velden in";
                IsGeneralErrorVisible = true;
                return false;
            }

            // Validatie: geldig email format
            if (!IsValidEmail(Email))
            {
                EmailError = "Voer een geldig e-mailadres in (bijv. naam@voorbeeld.nl)";
                IsEmailErrorVisible = true;
                GeneralError = "Controleer je gegevens";
                IsGeneralErrorVisible = true;
                return false;
            }

            // Validatie: wachtwoord minimaal 4 tekens
            if (Password.Length < 4)
            {
                PasswordError = "Wachtwoord moet minimaal 4 tekens bevatten";
                IsPasswordErrorVisible = true;
                GeneralError = "Controleer je gegevens";
                IsGeneralErrorVisible = true;
                return false;
            }

            // Validatie: wachtwoord geen ongeldige tekens
            if (!IsValidPassword(Password))
            {
                PasswordError = "Wachtwoord mag geen ongeldige tekens bevatten";
                IsPasswordErrorVisible = true;
                GeneralError = "Controleer je gegevens";
                IsGeneralErrorVisible = true;
                return false;
            }

            // Validatie: email bestaat al
            if (await _userRepository.EmailExistsAsync(Email.ToLower()))
            {
                EmailError = "Er bestaat al een account met dit e-mailadres";
                IsEmailErrorVisible = true;
                GeneralError = "Dit account bestaat al";
                IsGeneralErrorVisible = true;
                return false;
            }

            // Simuleer account aanmaken
            await Task.Delay(1000);

            // Registreer gebruiker in database
            var user = new User(Email.ToLower(), Password); // In productie: hash het wachtwoord!
            await _userRepository.AddAsync(user);

            // Trigger success event
            RegistrationSuccessful?.Invoke(this, EventArgs.Empty);
            return true;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Contains('\t') || password.Contains('\n') || password.Contains('\r'))
                return false;

            if (password.Length < 4)
                return false;

            return true;
        }
    }
}