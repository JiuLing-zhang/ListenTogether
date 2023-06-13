using ListenTogether.Business;
using ListenTogether.Business.Interfaces;
using ListenTogether.Data;
using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor
{
    public partial class App : Application
    {
        public App(IEnvironmentConfigService configService, DesktopNotification windowsNotification, DesktopMoving desktopMoving, IDeviceManage deviceManage)
        {
            InitializeComponent();

#if WINDOWS
            windowsNotification.OnSetTitle += (_, title) => WindowsTitleBarService.SetTitle(title);
            windowsNotification.OnWindowMinimize += (_, __) => WindowsTitleBarService.Minimize();
            windowsNotification.OnWindowMaximize += (_, __) => WindowsTitleBarService.Maximize();
            windowsNotification.OnWindowShowNormal += (_, __) => WindowsTitleBarService.ShowNormal();
            windowsNotification.OnWindowClose += (_, __) => WindowsTitleBarService.Close();

            desktopMoving.OnMouseDown += (_, __) => WindowsMoving.MouseDown();
            desktopMoving.OnMouseUp += (_, __) => WindowsMoving.MouseUp();
#endif

            //TODO 临时调试一下
            Settings.OSType = Config.Desktop ? OSTypeEnum.Desktop : OSTypeEnum.Phone;
            if (!Directory.Exists(GlobalPath.AppDataDirectory))
            {
                Directory.CreateDirectory(GlobalPath.AppDataDirectory);
            }
            if (!Directory.Exists(GlobalPath.MusicCacheDirectory))
            {
                Directory.CreateDirectory(GlobalPath.MusicCacheDirectory);
            }

            BusinessConfig.SetDataBaseConnection(Path.Combine(GlobalConfig.AppDataDirectory, GlobalConfig.LocalDatabaseName));
            var task = Task.Run(configService.ReadAllSettingsAsync);
            Settings.Environment = task.Result;
            DataConfig.SetWebApi(deviceManage.GetDeviceId());
            MainPage = new MainPage();
        }

    }
}