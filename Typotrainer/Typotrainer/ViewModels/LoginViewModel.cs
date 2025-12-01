using Domain.Repositories;
using System.Text.RegularExpressions;

namespace Typotrainer.ViewModels
{
    /// <summary>
    /// Login ViewModel - handles login logic with database
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
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

        // Event voor succesvolle login
        public event EventHandler? LoginSuccessful;

        public LoginViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> LoginAsync()
        {
            // Reset foutmelding
            IsErrorVisible = false;
            ErrorMessage = string.Empty;

            // Validatie e-mail
            if (string.IsNullOrWhiteSpace(Email))
            {
                ShowError("Voer een e-mailadres in");
                return false;
            }

            if (!IsValidEmail(Email))
            {
                ShowError("Voer een geldig e-mailadres in (bijv. naam@voorbeeld.nl)");
                return false;
            }

            // Validatie wachtwoord
            if (string.IsNullOrWhiteSpace(Password))
            {
                ShowError("Voer een wachtwoord in");
                return false;
            }

            if (Password.Length < 4)
            {
                ShowError("Wachtwoord moet minimaal 4 tekens bevatten");
                return false;
            }

            if (!IsValidPassword(Password))
            {
                ShowError("Wachtwoord mag geen ongeldige tekens bevatten");
                return false;
            }

            // Simuleer laadtijd (NFR1: binnen 2 seconden)
            await Task.Delay(2000);

            // Valideer login met database
            var user = await _userRepository.GetByEmailAsync(Email.ToLower());

            if (user == null)
            {
                ShowError("Ongeldige inloggegevens. Controleer je e-mail en wachtwoord.");
                return false;
            }

            // Simpele wachtwoord check (in productie zou dit gehashed zijn)
            if (user.PasswordHash != Password)
            {
                ShowError("Ongeldige inloggegevens. Controleer je e-mail en wachtwoord.");
                return false;
            }

            // Succesvolle login
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
            return true;
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            IsErrorVisible = true;
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