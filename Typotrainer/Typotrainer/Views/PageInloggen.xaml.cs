using System.Text.RegularExpressions;
using Typotrainer.Services;

namespace Typotrainer.Views;

public partial class PageInloggen : ContentView
{
    public PageInloggen()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Reset foutmelding
        ErrorLabel.IsVisible = false;
        ErrorLabel.Text = string.Empty;

        // Haal waarden op
        string email = EmailEntry.Text?.Trim() ?? string.Empty;
        string password = PasswordEntry.Text ?? string.Empty;

        // Validatie e-mail
        if (string.IsNullOrWhiteSpace(email))
        {
            ShowError("Voer een e-mailadres in");
            return;
        }

        if (!IsValidEmail(email))
        {
            ShowError("Voer een geldig e-mailadres in (bijv. naam@voorbeeld.nl)");
            return;
        }

        // Validatie wachtwoord
        if (string.IsNullOrWhiteSpace(password))
        {
            ShowError("Voer een wachtwoord in");
            return;
        }

        if (password.Length < 4)
        {
            ShowError("Wachtwoord moet minimaal 4 tekens bevatten");
            return;
        }

        if (!IsValidPassword(password))
        {
            ShowError("Wachtwoord mag geen ongeldige tekens bevatten");
            return;
        }

        // Toon loading indicator
        LoginButton.IsEnabled = false;
        LoginButton.Text = "Bezig met inloggen...";

        // Simuleer API call (2 seconden zoals in NFR1)
        await Task.Delay(2000);

        // Valideer login met UserService
        bool loginSuccess = UserService.ValidateLogin(email, password);

        if (loginSuccess)
        {
            // Succesvolle login
            await ShowSuccessMessage();

            // Navigeer naar dashboard
            var mainPage = GetMainPage();
            mainPage?.ShowDashboard();
        }
        else
        {
            ShowError("Ongeldige inloggegevens. Controleer je e-mail en wachtwoord.");
            LoginButton.IsEnabled = true;
            LoginButton.Text = "?  Inloggen";
        }
    }

    private void OnRegisterClicked(object sender, EventArgs e)
    {
        // Navigeer naar aanmeldpagina
        var mainPage = GetMainPage();
        mainPage?.ShowRegister();
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

    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }

    private async Task ShowSuccessMessage()
    {
        LoginButton.Text = "Ingelogd!";
        LoginButton.BackgroundColor = Colors.Green;
        await Task.Delay(1000);
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