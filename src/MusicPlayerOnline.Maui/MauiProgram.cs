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

            builder.Services.AddBusiness();

            builder
                .UseMauiApp<App>()
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