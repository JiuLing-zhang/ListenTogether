using Microsoft.Extensions.Configuration;
using CommunityToolkit.Maui;
using System.Reflection;

namespace ListenTogether
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("ListenTogether.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();


            builder.Configuration.AddConfiguration(config);

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBusiness()
                .ConfigureServices()
                .ConfigurePages()
                .ConfigureViewModels()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }
    }
}