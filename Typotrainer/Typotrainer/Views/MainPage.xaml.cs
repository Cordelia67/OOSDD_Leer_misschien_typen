namespace Typotrainer.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
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
}