namespace Typotrainer.Views;

public partial class MainPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public MainPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;

        SubPage.Content = new PageDashboard(); // Dashboard als startpagina
        HeaderTitel.Text = "Dashboard";
    }

    public void PageDashboardClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageDashboard();
        HeaderTitel.Text = "Dashboard";
    }

    public void PageOefeningClicked(object sender, EventArgs e)
    {
        // PageOefening komt van service provider
        SubPage.Content = _serviceProvider.GetService<PageOefening>();
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
}
