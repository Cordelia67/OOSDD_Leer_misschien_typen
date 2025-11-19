using System.Text.RegularExpressions;

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

        // Validatie
        if (string.IsNullOrWhiteSpace(email))
        {
            ShowError("Voer een e-mailadres in");
            return;
        }

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

        // Toon loading indicator
        LoginButton.IsEnabled = false;
        LoginButton.Text = "Bezig met inloggen...";

        // Simuleer API call (2 seconden zoals in NFR1)
        await Task.Delay(2000);

        // Voor demo: accepteer alle geldige combinaties
        // In een echte app zou hier authenticatie met een backend plaatsvinden
        bool loginSuccess = await ValidateLogin(email, password);

        if (loginSuccess)
        {
            // Succesvolle login
            await ShowSuccessMessage();

            // Navigeer naar dashboard
            // Dit kan worden aangepast afhankelijk van je navigatie structuur
            if (Parent is ContentView parentView &&
                parentView.Parent is VerticalStackLayout stack &&
                stack.Parent is ContentPage page)
            {
                // Zoek de MainPage en toon het dashboard
                var mainPage = page as MainPage;
                mainPage?.ShowDashboard();
            }
        }
        else
        {
            ShowError("Ongeldige gegevens");
            LoginButton.IsEnabled = true;
            LoginButton.Text = "?  Inloggen";
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
        // In een echte applicatie zou hier een API call naar je backend komen
        await Task.Delay(100);

        // Voor demo: accepteer alle geldige invoer
        return !string.IsNullOrWhiteSpace(email) && password.Length >= 4;
    }
}