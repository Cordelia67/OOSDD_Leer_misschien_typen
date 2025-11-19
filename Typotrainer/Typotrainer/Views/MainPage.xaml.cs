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
    void PageInloggenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageInloggen();
    }
    void PageAanmeldenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageAanmelden();
    }
    void PageDashboardClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageDashboard();
    }
    void PageOefeningClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageOefening();
    }
    void PageAdaptieveOefeningClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageAdaptieveOefening();
    }
    void PageResultatenClicked(object sender, EventArgs e)
    {
        SubPage.Content = new PageResultaten();
    }
    void PageInstellingenClicked(object sender, EventArgs e)
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
}
