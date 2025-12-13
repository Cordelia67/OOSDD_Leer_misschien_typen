using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Typotrainer.Services;
using Typotrainer.Core.Services;
using Typotrainer.Core.Interfaces;
using Typotrainer.Views;

namespace Typotrainer
{
    public static class MauiProgram
    {
        [SupportedOSPlatform("windows")]
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Registreer file provider
            builder.Services.AddSingleton<IFileProvider, MauiFileProvider>();

            // Registreer services van Core
            builder.Services.AddSingleton<TypingService>();
            builder.Services.AddSingleton<SentenceService>();

            // Registreer pagina's voor dependency injection
            builder.Services.AddTransient<PageOefening>(); // Transient: Nieuwe instantie elke keer
            builder.Services.AddSingleton<MainPage>(); // Singleton: Enkele instantie

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}