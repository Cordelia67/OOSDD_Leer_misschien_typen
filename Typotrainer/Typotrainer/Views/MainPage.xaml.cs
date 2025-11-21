namespace Typotrainer.Views;
using Typotrainer.Services;

public partial class MainPage : ContentPage
{
    readonly DatabaseService _db;   // add a field for your service

    public MainPage()
    {
        InitializeComponent();
        _db = new DatabaseService(); // or pass in your connection string here
    }

    // existing navigation handlers
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

    // NEW: run DB test automatically when page appears
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        DBLabel.Text = "Testing...";
        string result = await _db.DatabaseTest();
        DBLabel.Text = result;
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
