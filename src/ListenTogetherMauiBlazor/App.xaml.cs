using ListenTogether.Data.Api;
using ListenTogether.Data.Interface;
using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor
{
    public partial class App : Application
    {
        public App(IEnvironmentConfigService configService, IDeviceManage deviceManage)
        {
            InitializeComponent();

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

            var task = Task.Run(configService.ReadAllSettingsAsync);
            Settings.Environment = task.Result;

            var taskDeviceId = Task.Run(deviceManage.GetDeviceIdAsync);
            DataConfig.SetWebApi(taskDeviceId.Result);

            MainPage = new MainPage();
        }

    }
}