using Microsoft.Extensions.Configuration;
using MusicPlayerOnline.Business;
using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Maui.Pages;
namespace MusicPlayerOnline.Maui;
public partial class App : Application
{
    public App(IConfiguration config, IEnvironmentConfigService configService)
    {
        InitializeComponent();

        //文档文件夹
        if (!Directory.Exists(GlobalConfig.AppDataPath))
        {
            Directory.CreateDirectory(GlobalConfig.AppDataPath);
        }

        var appSetting = config.GetRequiredSection("AppSettings").Get<AppSettings>();

        //设备Id 自己生成
        string deviceInfoFileName = Path.Combine(GlobalConfig.AppDataPath, appSetting.DeviceInfoFileName);
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

        BusinessConfig.SetWebApi(Path.Combine(GlobalConfig.AppDataPath, appSetting.LocalDbName), appSetting.ApiDomain, deviceId);
        GlobalConfig.MyUserSetting = configService.ReadAllSettings();

        //TODO 临时赋值，实际需要判断登录状态
        BusinessConfig.IsUseApiInterface = false;

        //主题
        App.Current.UserAppTheme = GlobalConfig.MyUserSetting.General.IsDarkMode ? AppTheme.Dark : AppTheme.Light;

        Routing.RegisterRoute(nameof(PlayingPage), typeof(PlayingPage));
        Routing.RegisterRoute(nameof(MyFavoriteAddPage), typeof(MyFavoriteAddPage));        

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