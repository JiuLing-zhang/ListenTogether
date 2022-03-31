using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using MusicPlayerOnline.Business;
using MusicPlayerOnline.Maui.Pages;
using MusicPlayerOnline.Maui.ViewModels;

namespace MusicPlayerOnline.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            using var stream = File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            builder.Configuration.AddConfiguration(config);

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBusiness()
                .ConfigureEssentials()
                .ConfigurePages()
                .ConfigureViewModels()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            return builder.Build();
        }
    }
}