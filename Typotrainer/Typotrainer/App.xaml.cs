namespace Typotrainer
{
    public partial class App : Application
    {
        public App(IServiceProvider services)
        {
            InitializeComponent();
            MainPage = services.GetRequiredService<AppShell>();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            // Set venster grootte voor desktop platforms
            window.Width = 1200;
            window.Height = 800;
            window.MinimumWidth = 800;
            window.MinimumHeight = 600;

            return window;
        }
    }
}