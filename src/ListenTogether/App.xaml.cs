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
                    GlobalConfig.CurrentUser = null;
                }
                else
                {
                    userLocalService.UpdateToken(tokenInfo);
                }
                BusinessConfig.UserToken = tokenInfo;
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

        Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
        Routing.RegisterRoute(nameof(PlayingPage), typeof(PlayingPage));
        Routing.RegisterRoute(nameof(MyFavoriteDetailPage), typeof(MyFavoriteDetailPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(CacheCleanPage), typeof(CacheCleanPage));
        Routing.RegisterRoute(nameof(LogPage), typeof(LogPage));

        if (GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate)
        {
            //TODO 目前 MAUI 的线程切换好像存在 bug，不能直接弹窗，因此这里暂时只检查更新
            var taskUpdate = Task.Run(UpdateCheck.CheckNewVersionAsync);
            if (taskUpdate.Result)
            {
                ToastService.Show("发现新版本，请前往设置页更新。");
            }
        }
    }
}