using Domain.Models;
using Domain.Repositories;
using System.Text.RegularExpressions;

namespace Typotrainer.ViewModels
{
    /// <summary>
    /// ViewModel voor gebruikersregistratie
    /// Handles account aanmaken met real-time validatie
    /// </summary>
    public class RegisterViewModel : BaseViewModel
    {
        private const int MinPasswordLength = 4;
        private const int SimulatedLoadTimeMs = 1000;

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
                ValidateEmailAsync();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                ValidatePassword();
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

        public event EventHandler? RegistrationSuccessful;

        public RegisterViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterAsync()
        {
            ClearAllErrors();

            if (!ValidateAllFields())
                return false;

            if (!await ValidateEmailAvailability())
                return false;

            await SimulateAccountCreation();
            await CreateUserAccount();

            TriggerRegistrationSuccess();
            return true;
        }

        private async void ValidateEmailAsync()
        {
            ClearEmailError();

            if (!ShouldValidateEmail())
                return;

            if (!IsValidEmailFormat(Email))
            {
                ShowEmailError("Voer een geldig e-mailadres in");
                return;
            }

            await CheckEmailAvailability();
        }

        private void ValidatePassword()
        {
            ClearPasswordError();

            if (string.IsNullOrEmpty(Password))
                return;

            if (Password.Length < MinPasswordLength)
            {
                ShowPasswordError($"Wachtwoord moet minimaal {MinPasswordLength} tekens bevatten");
            }
            else if (!IsValidPasswordFormat(Password))
            {
                ShowPasswordError("Wachtwoord mag geen ongeldige tekens bevatten");
            }
        }

        private bool ValidateAllFields()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ShowEmailError("Voer een e-mailadres in");
                ShowGeneralError("Vul alle velden in");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ShowPasswordError("Voer een wachtwoord in");
                ShowGeneralError("Vul alle velden in");
                return false;
            }

            if (!IsValidEmailFormat(Email))
            {
                ShowEmailError("Voer een geldig e-mailadres in (bijv. naam@voorbeeld.nl)");
                ShowGeneralError("Controleer je gegevens");
                return false;
            }

            if (Password.Length < MinPasswordLength)
            {
                ShowPasswordError($"Wachtwoord moet minimaal {MinPasswordLength} tekens bevatten");
                ShowGeneralError("Controleer je gegevens");
                return false;
            }

            if (!IsValidPasswordFormat(Password))
            {
                ShowPasswordError("Wachtwoord mag geen ongeldige tekens bevatten");
                ShowGeneralError("Controleer je gegevens");
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateEmailAvailability()
        {
            if (await _userRepository.EmailExistsAsync(Email.ToLower()))
            {
                ShowEmailError("Er bestaat al een account met dit e-mailadres");
                ShowGeneralError("Dit account bestaat al");
                return false;
            }

            return true;
        }

        private async Task CheckEmailAvailability()
        {
            if (await _userRepository.EmailExistsAsync(Email.ToLower()))
            {
                ShowEmailError("Er bestaat al een account met dit e-mailadres");
            }
        }

        private async Task SimulateAccountCreation()
        {
            await Task.Delay(SimulatedLoadTimeMs);
        }

        private async Task CreateUserAccount()
        {
            // In productie: hash het wachtwoord met bcrypt/Argon2
            var user = new User(Email.ToLower(), Password);
            await _userRepository.AddAsync(user);
        }

        private void TriggerRegistrationSuccess()
        {
            RegistrationSuccessful?.Invoke(this, EventArgs.Empty);
        }

        private void ClearAllErrors()
        {
            ClearEmailError();
            ClearPasswordError();
            ClearGeneralError();
        }

        private void ClearEmailError()
        {
            IsEmailErrorVisible = false;
            IsGeneralErrorVisible = false;
        }

        private void ClearPasswordError()
        {
            IsPasswordErrorVisible = false;
            IsGeneralErrorVisible = false;
        }

        private void ClearGeneralError()
        {
            IsGeneralErrorVisible = false;
        }

        private void ShowEmailError(string message)
        {
            EmailError = message;
            IsEmailErrorVisible = true;
        }

        private void ShowPasswordError(string message)
        {
            PasswordError = message;
            IsPasswordErrorVisible = true;
        }

        private void ShowGeneralError(string message)
        {
            GeneralError = message;
            IsGeneralErrorVisible = true;
        }

        private bool ShouldValidateEmail()
        {
            return !string.IsNullOrEmpty(Email) && Email.Contains("@");
        }

        private bool IsValidEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                const string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPasswordFormat(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (ContainsInvalidCharacters(password))
                return false;

            return password.Length >= MinPasswordLength;
        }

        private bool ContainsInvalidCharacters(string password)
        {
            return password.Contains('\t')
                   || password.Contains('\n')
                   || password.Contains('\r');
        }
    }
}