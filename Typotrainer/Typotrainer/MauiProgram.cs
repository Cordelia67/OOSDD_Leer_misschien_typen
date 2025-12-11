using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Typotrainer.Services;
using Typotrainer.Core.Services;
using Typotrainer.Core.Interfaces;

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

            // Register file provider implementation
            builder.Services.AddSingleton<IFileProvider, MauiFileProvider>();

            // Register services from Core
            builder.Services.AddSingleton<TypingService>();
            builder.Services.AddSingleton<SentenceService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}