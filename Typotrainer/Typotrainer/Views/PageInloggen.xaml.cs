using System.Text.RegularExpressions;

namespace Typotrainer.Views;

public partial class PageInloggen : ContentView
{
    // Demo gebruikers database (in een echte app zou dit in een database of via API zijn)
    private static readonly Dictionary<string, string> DemoUsers = new()
    {
        { "test@test.nl", "test1234" },
        { "admin@typotrainer.nl", "admin123" },
        { "user@example.com", "password" },
        { "demo@demo.nl", "demo2024" }
    };

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

        // Valideer login met demo gebruikers
        bool loginSuccess = await ValidateLogin(email, password);

        if (loginSuccess)
        {
            // Succesvolle login
            await ShowSuccessMessage();

            // Navigeer naar dashboard
            if (Parent is ContentView parentView &&
                parentView.Parent is VerticalStackLayout stack &&
                stack.Parent is ContentPage page)
            {
                var mainPage = page as MainPage;
                mainPage?.ShowDashboard();
            }
        }
        else
        {
            ShowError("Ongeldige inloggegevens. Controleer je e-mail en wachtwoord.");
            LoginButton.IsEnabled = true;
            LoginButton.Text = "Inloggen";
        }
    }

    private void OnRegisterClicked(object sender, EventArgs e)
    {
        // Navigeer naar aanmeldpagina
        if (Parent is ContentView parentView &&
            parentView.Parent is VerticalStackLayout stack &&
            stack.Parent is ContentPage page)
        {
            var mainPage = page as MainPage;
            mainPage?.ShowRegister();
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

    private async Task<bool> ValidateLogin(string email, string password)
    {
        // Simuleer authenticatie
        await Task.Delay(100);

        // Controleer of e-mail en wachtwoord geldig zijn (format)
        if (!IsValidEmail(email) || !IsValidPassword(password))
            return false;

        // Controleer of gebruiker bestaat en wachtwoord klopt
        if (DemoUsers.TryGetValue(email.ToLower(), out string correctPassword))
        {
            // Vergelijk wachtwoord (case-sensitive)
            return password == correctPassword;
        }

        // Gebruiker niet gevonden
        return false;
    }
}