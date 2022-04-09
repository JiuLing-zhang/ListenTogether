using Microsoft.Extensions.Configuration;
namespace MusicPlayerOnline.Maui;
public partial class App : Application
{
    public App(IConfiguration config, IEnvironmentConfigService configService)
    {
        InitializeComponent();

        if (!Directory.Exists(GlobalConfig.AppDataDirectory))
        {
            Directory.CreateDirectory(GlobalConfig.AppDataDirectory);
        }
        if (!Directory.Exists(GlobalConfig.MusicCacheDirectory))
        {
            Directory.CreateDirectory(GlobalConfig.MusicCacheDirectory);
        }

        var appSetting = config.GetRequiredSection("AppSettings").Get<AppSettings>();

        //设备Id 自己生成
        string deviceInfoFileName = Path.Combine(GlobalConfig.AppDataDirectory, appSetting.DeviceInfoFileName);
        string deviceId;
        if (!File.Exists(deviceInfoFileName))
        {
            deviceId = JiuLing.CommonLibs.GuidUtils.GetFormatDefault();
            File.WriteAllText(deviceInfoFileName, deviceId);
        }
        else
        {
            deviceId = File.ReadAllText(deviceInfoFileName);
        }

        BusinessConfig.SetWebApi(Path.Combine(GlobalConfig.AppDataDirectory, appSetting.LocalDbName), appSetting.ApiDomain, deviceId);
        GlobalConfig.MyUserSetting = configService.ReadAllSettings();

        //TODO 临时赋值，实际需要判断登录状态
        BusinessConfig.IsUseApiInterface = false;

        //主题
        //App.Current.UserAppTheme = GlobalConfig.MyUserSetting.General.IsDarkMode ? AppTheme.Dark : AppTheme.Light;


        if (Config.Desktop)
        {
            MainPage = new DesktopShell();
        }
        else
        {
            MainPage = new MobileShell();
        }

        Routing.RegisterRoute(nameof(PlayingPage), typeof(PlayingPage));
        Routing.RegisterRoute(nameof(MyFavoriteAddPage), typeof(MyFavoriteAddPage));
        Routing.RegisterRoute(nameof(MyFavoriteEditPage), typeof(MyFavoriteEditPage));
        Routing.RegisterRoute(nameof(MyFavoriteDetailPage), typeof(MyFavoriteDetailPage));
        Routing.RegisterRoute(nameof(AddToMyFavoritePage), typeof(AddToMyFavoritePage));
    }
}