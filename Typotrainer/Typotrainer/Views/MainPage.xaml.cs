using Typotrainer.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Typotrainer.Views;

public partial class MainPage : ContentPage
{
    private readonly IServiceProvider? _services;

    public MainPage(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;

        // Start met login pagina
        ShowLogin();
    }

    // Parameterloze constructor voor XAML - indien nodig
    public MainPage()
    {
        InitializeComponent();
        _services = null;
    }

    public void PageInloggenClicked(object sender, EventArgs e)
    {
        ShowLogin();
    }

    public void PageAanmeldenClicked(object sender, EventArgs e)
    {
        ShowRegister();
    }

    public void PageDashboardClicked(object sender, EventArgs e)
    {
        ShowDashboard();
    }

    public void PageOefeningClicked(object sender, EventArgs e)
    {
        ShowOefening();
    }

    public void PageAdaptieveOefeningClicked(object sender, EventArgs e)
    {
        ShowAdaptieveOefening();
    }

    public void PageResultatenClicked(object sender, EventArgs e)
    {
        ShowResultaten();
    }

    public void PageInstellingenClicked(object sender, EventArgs e)
    {
        ShowInstellingen();
    }

    // Publieke methodes voor navigatie vanuit andere pagina's
    public void ShowDashboard()
    {
        SubPage.Content = new PageDashboard();
    }

    public void ShowRegister()
    {
        if (_services == null) return;

        var viewModel = _services.GetRequiredService<RegisterViewModel>();
        SubPage.Content = new PageAanmelden(viewModel);
    }

    public void ShowLogin()
    {
        if (_services == null) return;

        var viewModel = _services.GetRequiredService<LoginViewModel>();
        SubPage.Content = new PageInloggen(viewModel);
    }

    public void ShowOefening()
    {
        if (_services == null) return;

        var viewModel = _services.GetRequiredService<ExerciseViewModel>();
        SubPage.Content = new PageOefening(viewModel);
    }

    private void ShowAdaptieveOefening()
    {
        SubPage.Content = new PageAdaptieveOefening();
    }

    private void ShowResultaten()
    {
        SubPage.Content = new PageResultaten();
    }

    private void ShowInstellingen()
    {
        SubPage.Content = new PageInstellingen();
    }
}