namespace Typotrainer.Views;
using Typotrainer.Services;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        SubPage.Content = new PageDashboard(); // Dashboard als startpagina
    }

    public void PageInloggenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageInloggen();
    }

    public void PageAanmeldenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageAanmelden();
    }

    public void PageDashboardClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageDashboard();
    }

    public void PageOefeningClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageOefening();
    }

    public void PageAdaptieveOefeningClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageAdaptieveOefening();
    }

    public void PageResultatenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageResultaten();
    }

    public void PageInstellingenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageInstellingen();
    }

    // Publieke methodes voor navigatie vanuit andere pagina's
    public void ShowDashboard()
    {
        SubPage.Content = new PageDashboard();
    }

    public void ShowRegister()
    {
        SubPage.Content = new PageAanmelden();
    }

    public void ShowLogin()
    {
        SubPage.Content = new PageInloggen();
    }
}
