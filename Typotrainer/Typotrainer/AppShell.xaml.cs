using Typotrainer.Views;

namespace Typotrainer
{
    public partial class AppShell : Shell
    {
        public AppShell(IServiceProvider services)
        {
            InitializeComponent();

            // Laad MainPage via DI
            var mainPage = services.GetRequiredService<MainPage>();

            Items.Add(new ShellContent
            {
                Title = "Home",
                Route = "MainPage",
                Content = mainPage
            });
        }
    }
}