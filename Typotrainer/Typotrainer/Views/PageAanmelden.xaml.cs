using System.Text.RegularExpressions;
using Typotrainer.Services;

namespace Typotrainer.Views;

public partial class PageAanmelden : ContentView
{
    public PageAanmelden()
    {
        InitializeComponent();
    }

    // Real-time validatie bij het typen van email
    private void OnEmailChanged(object sender, TextChangedEventArgs e)
    {
        string email = e.NewTextValue?.Trim() ?? "";

        EmailErrorLabel.IsVisible = false;
        ErrorLabel.IsVisible = false;

        // Controleer email format
        if (!string.IsNullOrEmpty(email) && email.Contains("@"))
        {
            // Check of email geldig is
            if (!IsValidEmail(email))
            {
                EmailErrorLabel.Text = "Voer een geldig e-mailadres in";
                EmailErrorLabel.IsVisible = true;
                return;
            }

            // Controleer of email al bestaat
            if (UserService.UserExists(email))
            {
                EmailErrorLabel.Text = "Er bestaat al een account met dit e-mailadres";
                EmailErrorLabel.IsVisible = true;
            }
        }
    }

    // Real-time validatie bij het typen van wachtwoord
    private void OnPasswordChanged(object sender, TextChangedEventArgs e)
    {
        string password = e.NewTextValue ?? "";

        PasswordErrorLabel.IsVisible = false;
        ErrorLabel.IsVisible = false;

        if (!string.IsNullOrEmpty(password))
        {
            if (password.Length < 4)
            {
                PasswordErrorLabel.Text = "Wachtwoord moet minimaal 4 tekens bevatten";
                PasswordErrorLabel.IsVisible = true;
            }
            else if (!IsValidPassword(password))
            {
                PasswordErrorLabel.Text = "Wachtwoord mag geen ongeldige tekens bevatten";
                PasswordErrorLabel.IsVisible = true;
            }
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Verberg foutmeldingen
        ErrorLabel.IsVisible = false;
        EmailErrorLabel.IsVisible = false;
        PasswordErrorLabel.IsVisible = false;

        // Haal waarden op
        string email = EmailEntry.Text?.Trim() ?? "";
        string password = PasswordEntry.Text ?? "";

        // Validatie: velden ingevuld
        if (string.IsNullOrWhiteSpace(email))
        {
            EmailErrorLabel.Text = "Voer een e-mailadres in";
            EmailErrorLabel.IsVisible = true;
            ErrorLabel.Text = "Vul alle velden in";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            PasswordErrorLabel.Text = "Voer een wachtwoord in";
            PasswordErrorLabel.IsVisible = true;
            ErrorLabel.Text = "Vul alle velden in";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Validatie: geldig email format
        if (!IsValidEmail(email))
        {
            EmailErrorLabel.Text = "Voer een geldig e-mailadres in (bijv. naam@voorbeeld.nl)";
            EmailErrorLabel.IsVisible = true;
            ErrorLabel.Text = "Controleer je gegevens";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Validatie: wachtwoord minimaal 4 tekens
        if (password.Length < 4)
        {
            PasswordErrorLabel.Text = "Wachtwoord moet minimaal 4 tekens bevatten";
            PasswordErrorLabel.IsVisible = true;
            ErrorLabel.Text = "Controleer je gegevens";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Validatie: wachtwoord geen ongeldige tekens
        if (!IsValidPassword(password))
        {
            PasswordErrorLabel.Text = "Wachtwoord mag geen ongeldige tekens bevatten";
            PasswordErrorLabel.IsVisible = true;
            ErrorLabel.Text = "Controleer je gegevens";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Validatie: email bestaat al
        if (UserService.UserExists(email))
        {
            EmailErrorLabel.Text = "Er bestaat al een account met dit e-mailadres";
            EmailErrorLabel.IsVisible = true;
            ErrorLabel.Text = "Dit account bestaat al";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Toon loading
        RegisterButton.Text = "Account aanmaken...";
        RegisterButton.IsEnabled = false;
        BackToLoginButton.IsEnabled = false;

        // Simuleer account aanmaken
        await Task.Delay(1000);

        // Registreer gebruiker
        bool success = UserService.RegisterUser(email, password);

        if (success)
        {
            // Toon success modal
            ShowSuccessModal();
        }
        else
        {
            // Dit zou normaal niet moeten gebeuren (is al gecontroleerd)
            ErrorLabel.Text = "Er ging iets mis. Probeer opnieuw.";
            ErrorLabel.IsVisible = true;
            RegisterButton.Text = "Account aanmaken";
            RegisterButton.IsEnabled = true;
            BackToLoginButton.IsEnabled = true;
        }
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Regex patroon voor e-mailvalidatie
            // Accepteert: naam@domein.extensie
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

        // Wachtwoord mag geen tabs of newlines bevatten
        if (password.Contains('\t') || password.Contains('\n') || password.Contains('\r'))
            return false;

        // Minimaal 4 tekens (wordt al eerder gecheckt, maar voor zekerheid)
        if (password.Length < 4)
            return false;

        return true;
    }

    private void ShowSuccessModal()
    {
        // Verberg registratie formulier
        RegisterFrame.IsVisible = false;

        // Toon success modal
        SuccessFrame.IsVisible = true;
    }

    private void OnBackToLoginClicked(object sender, EventArgs e)
    {
        NavigateToLogin();
    }

    private void OnGoToLoginClicked(object sender, EventArgs e)
    {
        NavigateToLogin();
    }

    private void NavigateToLogin()
    {
        var mainPage = GetMainPage();
        mainPage?.ShowLogin();
    }

    private MainPage? GetMainPage()
    {
        Element? current = this.Parent;
        while (current != null)
        {
            if (current is MainPage mainPage)
                return mainPage;
            current = current.Parent;
        }
        return null;
    }
}