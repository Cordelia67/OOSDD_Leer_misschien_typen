using Typotrainer.ViewModels;

namespace Typotrainer.Views;

public partial class PageInloggen : ContentView
{
    private readonly LoginViewModel _viewModel;

    public PageInloggen(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Subscribe to login success event
        _viewModel.LoginSuccessful += OnLoginSuccessful;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Disable button during login
        LoginButton.IsEnabled = false;
        LoginButton.Text = "Bezig met inloggen...";

        bool success = await _viewModel.LoginAsync();

        if (success)
        {
            // Show success message
            LoginButton.Text = "Ingelogd!";
            LoginButton.BackgroundColor = Colors.Green;
            await Task.Delay(1000);
        }
        else
        {
            // Re-enable button
            LoginButton.IsEnabled = true;
            LoginButton.Text = "→  Inloggen";
        }
    }

    private void OnLoginSuccessful(object? sender, EventArgs e)
    {
        // Navigate to dashboard
        var mainPage = GetMainPage();
        mainPage?.ShowDashboard();
    }

    private void OnRegisterClicked(object sender, EventArgs e)
    {
        var mainPage = GetMainPage();
        mainPage?.ShowRegister();
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