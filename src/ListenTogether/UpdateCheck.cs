using JiuLing.CommonLibs.Net;

namespace ListenTogether;

public class UpdateCheck
{
    private readonly HttpClientHelper MyHttpClient = new HttpClientHelper();

    private readonly ILogger<UpdateCheck> _logger;
    public UpdateCheck(ILogger<UpdateCheck> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 自动更新地址
    /// </summary>
    private string CheckUpdateUrl
    {
        get
        {
            if (GlobalConfig.UpdateDomain.IsEmpty())
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
            return $"{GlobalConfig.UpdateDomain}/{osTag}";
        }
    }

    public async Task DoAsync(bool isBackgroundCheck)
    {
        await Task.Run(async () =>
        {
            try
            {
                string json = await MyHttpClient.GetReadString(CheckUpdateUrl);
                var obj = json.ToObject<JiuLing.CommonLibs.Model.AppUpgradeInfo>();
                if (obj == null)
                {
                    if (!isBackgroundCheck)
                    {
                        await ToastService.Show("检查失败，连接服务器失败");
                    }
                    else
                    {
                        _logger.LogError(new Exception("连接服务器失败"), "自动更新检查失败");
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

                var (isNeedUpdate, isAllowRun) = JiuLing.CommonLibs.VersionUtils.CheckNeedUpdate(GlobalConfig.CurrentVersionString, version, minVersion);

                async void CheckUpdateInner()
                {
                    if (isNeedUpdate == false)
                    {
                        if (!isBackgroundCheck)
                        {
                            await ToastService.Show("当前版本为最新版");
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
                                await ToastService.Show("启动浏览器失败，请重试");
                                _logger.LogError(ex, "打开链接失败。");
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
                                await ToastService.Show("启动浏览器失败，请重试");
                                _logger.LogError(ex, "打开链接失败。");
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
                    await ToastService.Show($"检查失败：{ex.Message}");
                }
                else
                {
                    _logger.LogError(ex, "自动更新检查失败");
                }
            }
        });
    }

}