using Domain.Repositories;
using System.Text.RegularExpressions;

namespace Typotrainer.ViewModels
{
    /// <summary>
    /// ViewModel voor login functionaliteit
    /// Handles authenticatie van gebruikers
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        private const int MinPasswordLength = 4;
        private const int SimulatedLoadTimeMs = 2000;

        private readonly IUserRepository _userRepository;

        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isErrorVisible = false;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set => SetProperty(ref _isErrorVisible, value);
        }

        public event EventHandler? LoginSuccessful;

        public LoginViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> LoginAsync()
        {
            ClearErrors();

            if (!ValidateInput())
                return false;

            await SimulateLoading();

            return await AuthenticateUser();
        }

        private void ClearErrors()
        {
            IsErrorVisible = false;
            ErrorMessage = string.Empty;
        }

        private bool ValidateInput()
        {
            if (!ValidateEmail())
                return false;

            if (!ValidatePassword())
                return false;

            return true;
        }

        private bool ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ShowError("Voer een e-mailadres in");
                return false;
            }

            if (!IsValidEmailFormat(Email))
            {
                ShowError("Voer een geldig e-mailadres in (bijv. naam@voorbeeld.nl)");
                return false;
            }

            return true;
        }

        private bool ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                ShowError("Voer een wachtwoord in");
                return false;
            }

            if (Password.Length < MinPasswordLength)
            {
                ShowError($"Wachtwoord moet minimaal {MinPasswordLength} tekens bevatten");
                return false;
            }

            if (!IsValidPasswordFormat(Password))
            {
                ShowError("Wachtwoord mag geen ongeldige tekens bevatten");
                return false;
            }

            return true;
        }

        private async Task SimulateLoading()
        {
            await Task.Delay(SimulatedLoadTimeMs);
        }

        private async Task<bool> AuthenticateUser()
        {
            var user = await _userRepository.GetByEmailAsync(Email.ToLower());

            if (user == null || !IsPasswordCorrect(user.PasswordHash))
            {
                ShowError("Ongeldige inloggegevens. Controleer je e-mail en wachtwoord.");
                return false;
            }

            TriggerLoginSuccess();
            return true;
        }

        private bool IsPasswordCorrect(string storedHash)
        {
            // In productie: gebruik proper password hashing (bcrypt, Argon2, etc.)
            return storedHash == Password;
        }

        private void TriggerLoginSuccess()
        {
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            IsErrorVisible = true;
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

            if (password.Length < MinPasswordLength)
                return false;

            return true;
        }

        private bool ContainsInvalidCharacters(string password)
        {
            return password.Contains('\t')
                   || password.Contains('\n')
                   || password.Contains('\r');
        }
    }
}