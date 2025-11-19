namespace Typotrainer.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

    }
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
}
