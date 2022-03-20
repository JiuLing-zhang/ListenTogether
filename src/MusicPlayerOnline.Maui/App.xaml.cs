using MusicPlayerOnline.Maui.Pages;
namespace MusicPlayerOnline.Maui;
public partial class App : Application
{
    public App()
    {
        InitializeComponent();


        if (Config.Desktop)
        {
            MainPage = new DesktopShell();
        }
        else
        {
            MainPage = new MobileShell();
        }


        //Routing.RegisterRoute(nameof(PlaylistPage), typeof(PlaylistPage));
        //Routing.RegisterRoute(nameof(MyFavoritePage), typeof(MyFavoritePage));
        //Routing.RegisterRoute(nameof(PlayingPage), typeof(PlayingPage));
        //Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
    }
}