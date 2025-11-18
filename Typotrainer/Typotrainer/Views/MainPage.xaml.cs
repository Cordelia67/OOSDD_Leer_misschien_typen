namespace Typotrainer.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

    }
		void OnPage1Clicked(object sender, EventArgs e)
		{
			SubPage.Content = new Page1();
		}
        void OnPage2Clicked(object sender, EventArgs e)
        {
            SubPage.Content = new Page2();
        }
}
