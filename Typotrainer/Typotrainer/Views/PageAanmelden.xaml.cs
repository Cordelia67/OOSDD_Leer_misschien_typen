using Typotrainer.ViewModels;

namespace Typotrainer.Views;

public partial class PageAanmelden : ContentView
{
    private readonly RegisterViewModel _viewModel;

    public PageAanmelden(RegisterViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Subscribe to registration success event
        _viewModel.RegistrationSuccessful += OnRegistrationSuccessful;
    }

    // Deze methods zijn niet meer nodig - real-time validatie zit nu in ViewModel
    private void OnEmailChanged(object sender, TextChangedEventArgs e)
    {
        // ViewModel handelt dit af via property binding
    }

    private void OnPasswordChanged(object sender, TextChangedEventArgs e)
    {
        // ViewModel handelt dit af via property binding
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Disable buttons during registration
        RegisterButton.Text = "Account aanmaken...";
        RegisterButton.IsEnabled = false;
        BackToLoginButton.IsEnabled = false;

        bool success = await _viewModel.RegisterAsync();

        if (success)
        {
            // Show success modal
            ShowSuccessModal();
        }
        else
        {
            // Re-enable buttons
            RegisterButton.Text = "Account aanmaken";
            RegisterButton.IsEnabled = true;
            BackToLoginButton.IsEnabled = true;
        }
    }

    private void OnRegistrationSuccessful(object? sender, EventArgs e)
    {
        // Event is triggered, modal will be shown
    }

    private void ShowSuccessModal()
    {
        RegisterFrame.IsVisible = false;
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