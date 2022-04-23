using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace MusicPlayerOnline.Maui;
public partial class App : Application
{
    public App(IConfiguration config, IEnvironmentConfigService configService, IUserLocalService userLocalService)
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

        JsonExtension.DefaultOptions = new System.Text.Json.JsonSerializerOptions()
        {
            PropertyNamingPolicy = null,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

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

        BusinessConfig.TokenUpdated += (_, _) => userLocalService.UpdateToken(BusinessConfig.UserToken);
        BusinessConfig.SetWebApi(Path.Combine(GlobalConfig.AppDataDirectory, appSetting.LocalDbName), appSetting.ApiDomain, deviceId);
        GlobalConfig.CurrentUser = userLocalService.Read();

        GlobalConfig.MyUserSetting = configService.ReadAllSettings();

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
        Routing.RegisterRoute(nameof(MyFavoriteDetailPage), typeof(MyFavoriteDetailPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(CacheCleanPage), typeof(CacheCleanPage));
        Routing.RegisterRoute(nameof(LogPage), typeof(LogPage));
    }
}