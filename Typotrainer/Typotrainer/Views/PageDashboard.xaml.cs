namespace Typotrainer.Views;

public partial class PageDashboard : ContentView
{
    public PageDashboard()
    {
        InitializeComponent();
    }

    private void PageOefeningClicked(object sender, EventArgs e)
    {
        GetMainPage()?.PageOefeningClicked(sender, e);
    }

    private void PageResultatenClicked(object sender, EventArgs e)
    {
        GetMainPage()?.PageResultatenClicked(sender, e);
    }

    private void PageAdaptieveOefeningClicked(object sender, EventArgs e)
    {
        GetMainPage()?.PageAdaptieveOefeningClicked(sender, e);
    }

    private void PageInstellingenClicked(object sender, EventArgs e)
    {
        GetMainPage()?.PageInstellingenClicked(sender, e);
    }

    private MainPage GetMainPage()
    {
        Element current = this.Parent;
        while (current != null)
        {
            if (current is MainPage mainPage)
                return mainPage;
            current = current.Parent;
        }
        return null;
    }
}