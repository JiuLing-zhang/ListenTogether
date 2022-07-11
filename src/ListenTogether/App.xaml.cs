using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace ListenTogether;
public partial class App : Application
{
    public App(IConfiguration config, IEnvironmentConfigService configService, IUserLocalService userLocalService)
    {
        InitializeComponent();

        Logger.Info("系统启动");

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

        GlobalConfig.AppSettings = config.GetRequiredSection("AppSettings").Get<AppSettings>();

        GlobalConfig.CurrentVersion = AppInfo.Current.Version;

        BusinessConfig.SetDataBaseConnection(Path.Combine(GlobalConfig.AppDataDirectory, GlobalConfig.LocalDatabaseName));
        if (GlobalConfig.AppSettings.ApiDomain.IsNotEmpty())
        {
            BusinessConfig.TokenUpdated += (_, tokenInfo) =>
            {
                if (tokenInfo == null)
                {
                    userLocalService.Remove();
                }
                else
                {
                    userLocalService.UpdateToken(tokenInfo);
                }
                GlobalConfig.CurrentUser = null;
            };

            string deviceId = Preferences.Get("DeviceId", "");
            if (deviceId.IsEmpty())
            {
                deviceId = JiuLing.CommonLibs.GuidUtils.GetFormatDefault();
                Preferences.Set("DeviceId", deviceId);
            }

            BusinessConfig.SetWebApi(GlobalConfig.AppSettings.ApiDomain, deviceId);
            GlobalConfig.CurrentUser = userLocalService.Read();
        }

        var task = Task.Run(configService.ReadAllSettingsAsync);
        GlobalConfig.MyUserSetting = task.Result;

        App.Current.UserAppTheme = GlobalConfig.MyUserSetting.General.IsDarkMode ? AppTheme.Dark : AppTheme.Light;

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
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(CacheCleanPage), typeof(CacheCleanPage));
        Routing.RegisterRoute(nameof(LogPage), typeof(LogPage));
    }
}