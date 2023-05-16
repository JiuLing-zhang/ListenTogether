using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Net;
using ListenTogether.EasyLog;
using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
public class AutoUpgrade : IAutoUpgrade
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAppVersion _appVersion;
    private readonly NetConfig _netConfig;
    public AutoUpgrade(IHttpClientFactory httpClientFactory, IAppVersion appVersion, NetConfig netConfig)
    {
        _httpClientFactory = httpClientFactory;
        _appVersion = appVersion;
        _netConfig = netConfig;
    }

    /// <summary>
    /// 自动更新地址
    /// </summary>
    private string CheckUpdateUrl
    {
        get
        {
            if (_netConfig.UpdateDomain.IsEmpty())
            {
                throw new Exception("更新服务器未配置");
            }

            string osTag;
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                osTag = "windows";
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                osTag = "android";
            }
            else
            {
                throw new ArgumentException("不支持的系统类型");
            }
            return $"{_netConfig.UpdateDomain}/{osTag}";
        }
    }

    public async Task DoAsync(bool isBackgroundCheck)
    {
        try
        {
            string json = await _httpClientFactory.CreateClient().GetStringAsync(CheckUpdateUrl);
            var obj = json.ToObject<JiuLing.CommonLibs.Model.AppUpgradeInfo>();
            if (obj == null)
            {
                if (!isBackgroundCheck)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await App.Current.MainPage.DisplayAlert("提示", "检查失败，连接服务器失败", "确定");
                    });
                }
                else
                {
                    Logger.Error("自动更新检查失败", new Exception("连接服务器失败"));
                }
                return;
            }

            string version;
            string minVersion;
#if ANDROID
                version = obj.Version[..obj.Version.LastIndexOf(".")];
                minVersion = obj.MinVersion[..obj.MinVersion.LastIndexOf(".")];
#else
            version = obj.Version;
            minVersion = obj.MinVersion;
#endif

            var (isNeedUpdate, isAllowRun) = JiuLing.CommonLibs.VersionUtils.CheckNeedUpdate(_appVersion.GetCurrentVersionString(), version, minVersion);

            async void CheckUpdateInner()
            {
                if (isNeedUpdate == false)
                {
                    if (!isBackgroundCheck)
                    {
                        await App.Current.MainPage.DisplayAlert("提示", "当前版本为最新版", "确定");
                    }
                    return;
                }

                string message = $"发现新版本 {obj.Version}";
                if (isAllowRun)
                {
                    bool isDoUpdate = await App.Current.MainPage.DisplayAlert("提示", message, "下载", "忽略");
                    if (isDoUpdate == true)
                    {
                        try
                        {
                            await Browser.Default.OpenAsync(obj.DownloadUrl.ToUri(), BrowserLaunchMode.SystemPreferred);
                        }
                        catch (Exception ex)
                        {
                            await App.Current.MainPage.DisplayAlert("提示", "启动浏览器失败，请重试", "确定");
                            Logger.Error("打开链接失败。", ex);
                        }
                    }
                }
                else
                {
                    bool isDoUpdate = await App.Current.MainPage.DisplayAlert("当前版本已停用", message, "下载并退出", "退出");
                    if (isDoUpdate == true)
                    {
                        try
                        {
                            await Browser.Default.OpenAsync(obj.DownloadUrl.ToUri(), BrowserLaunchMode.External);
                        }
                        catch (Exception ex)
                        {
                            await App.Current.MainPage.DisplayAlert("提示", "启动浏览器失败，请重试", "确定");
                            Logger.Error("打开链接失败。", ex);
                        }
                    }
                    Application.Current.Quit();
                }
            }
            MainThread.BeginInvokeOnMainThread(CheckUpdateInner);
        }
        catch (Exception ex)
        {
            if (!isBackgroundCheck)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("提示", $"检查失败：{ex.Message}", "确定");
                });
            }
            else
            {
                Logger.Error("自动更新检查失败", ex);
            }
        }
    }
}