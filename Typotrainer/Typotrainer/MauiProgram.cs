using Microsoft.Extensions.Logging;
using Domain.Repositories;
using Domain.Services;
using DB;
using DB.Services;
using Typotrainer.ViewModels;
using Typotrainer.Views;

namespace Typotrainer
{
    public static class MauiProgram
    {
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

            // === TIJDELIJKE IN-MEMORY REPOSITORIES ===
            // Later te vervangen door database implementaties
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IExerciseRepository, ExerciseRepository>();

            // === Services ===
            builder.Services.AddSingleton<ISentenceService, SentenceService>();
            builder.Services.AddSingleton<ITypingService, TypingService>();

            // === ViewModels ===
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<ExerciseViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();

            // === Views ===
            builder.Services.AddTransient<PageInloggen>();
            builder.Services.AddTransient<PageAanmelden>();
            builder.Services.AddTransient<PageOefening>();
            builder.Services.AddTransient<PageDashboard>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}