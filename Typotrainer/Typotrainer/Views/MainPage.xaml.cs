namespace Typotrainer.Views;
using Typotrainer.Services;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        SubPage.Content = new PageDashboard(); // Dashboard als startpagina
        HeaderTitel.Text = "Dashboard";
    }

    public void PageInloggenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageInloggen();
        HeaderTitel.Text = "Inloggen";
    }

    public void PageAanmeldenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageAanmelden();
        HeaderTitel.Text = "Aanmelden";
    }

    public void PageDashboardClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageDashboard();
        HeaderTitel.Text = "Dashboard";
    }

    public void PageOefeningClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageOefening();
        HeaderTitel.Text = "Oefening";
    }

    public void PageAdaptieveOefeningClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageAdaptieveOefening();
        HeaderTitel.Text = "Adaptieve oefening";
    }

    public void PageResultatenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageResultaten();
        HeaderTitel.Text = "Resultaten";
    }

    public void PageInstellingenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageInstellingen();
        HeaderTitel.Text = "Instellingen";
    }

    // Publieke methodes voor navigatie vanuit andere pagina's
    public void ShowDashboard()
    {
        SubPage.Content = new PageDashboard();
        HeaderTitel.Text = "Dashboard";
    }

    public void ShowRegister()
    {
        SubPage.Content = new PageAanmelden();
        HeaderTitel.Text = "Aanmelden";
    }

    public void ShowLogin()
    {
        SubPage.Content = new PageInloggen();
        HeaderTitel.Text = "Inloggen";
    }
}
