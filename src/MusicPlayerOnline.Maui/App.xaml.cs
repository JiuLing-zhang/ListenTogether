using MusicPlayerOnline.Business;
using MusicPlayerOnline.Business.Factories;
using MusicPlayerOnline.Maui.Pages;
namespace MusicPlayerOnline.Maui;
public partial class App : Application
{
    public App(IUserConfigServiceFactory userConfigServiceFactory)
    {
        InitializeComponent();

        BusinessConfig.IsUseApiInterface = false;
        if (!Directory.Exists(GlobalConfig.AppDataPath))
        {
            Directory.CreateDirectory(GlobalConfig.AppDataPath);
        }
        BusinessConfig.SetWebApi(Path.Combine(GlobalConfig.AppDataPath, "test.db"), "", "123");

        GlobalConfig.MyUserSetting = userConfigServiceFactory.Create().ReadAllSettings();

        if (Config.Desktop)
        {
            MainPage = new DesktopShell();
        }
        else
        {
            MainPage = new MobileShell();
        }
    }
}