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

            RegisterRepositories(builder.Services);
            RegisterServices(builder.Services);
            RegisterViewModels(builder.Services);
            RegisterViews(builder.Services);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IExerciseRepository, ExerciseRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ISentenceService, SentenceService>();
            services.AddSingleton<ITypingService, TypingService>();
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<ExerciseViewModel>();
            services.AddTransient<DashboardViewModel>();
        }

        private static void RegisterViews(IServiceCollection services)
        {
            // Shell en App
            services.AddSingleton<AppShell>();
            services.AddSingleton<App>();

            // Pages
            services.AddTransient<MainPage>();
            services.AddTransient<PageInloggen>();
            services.AddTransient<PageAanmelden>();
            services.AddTransient<PageOefening>();
            services.AddTransient<PageDashboard>();
        }
    }
}