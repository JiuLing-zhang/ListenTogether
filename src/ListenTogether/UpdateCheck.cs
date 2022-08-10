using JiuLing.CommonLibs.Net;

namespace ListenTogether;

internal class UpdateCheck
{
    private readonly static HttpClientHelper _httpClient = new HttpClientHelper();

    /// <summary>
    /// App Key
    /// </summary>
    private const string AppKey = "listen-together";

    /// <summary>
    /// 自动更新地址
    /// </summary>
    private static string CheckUpdateUrl
    {
        get
        {
            if (GlobalConfig.AppSettings.UpdateDomain.IsEmpty())
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
            return $"{GlobalConfig.AppSettings.UpdateDomain}/api/app/{AppKey}/{osTag}";
        }
    }

    public static async Task Do(bool isBackgroundCheck)
    {
        await Task.Run(async () =>
        {
            try
            {
                string json = await _httpClient.GetReadString(CheckUpdateUrl);
                var obj = json.ToObject<JiuLing.CommonLibs.Model.AppUpgradeInfo>();
                if (obj == null)
                {
                    if (!isBackgroundCheck)
                    {
                        await ToastService.Show("检查失败，连接服务器失败");
                    }
                    else
                    {
                        Logger.Error("自动更新检查失败", new Exception("连接服务器失败"));
                    }
                    return;
                }
                //TODO Android版本号比对不正确
                var (isNeedUpdate, isAllowRun) = JiuLing.CommonLibs.VersionUtils.CheckNeedUpdate(GlobalConfig.CurrentVersionString, obj.Version, obj.MinVersion);
                if (isNeedUpdate == false)
                {
                    if (!isBackgroundCheck)
                    {
                        await ToastService.Show("当前版本为最新版");
                    }
                    return;
                }

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    string message = "";
                    if (isAllowRun == false)
                    {
                        message = $"当前版本已过期！{Environment.NewLine}";
                    }
                    message = $"{message}发现新版本 {obj.Version}，确认要下载吗？";

                    var isUpdate = await App.Current.MainPage.DisplayAlert("提示", message, "确定", "取消");
                    if (isUpdate == false)
                    {
                        return;
                    }

                    try
                    {
                        await Browser.Default.OpenAsync(obj.DownloadUrl.ToUri(), BrowserLaunchMode.SystemPreferred);
                    }
                    catch (Exception ex)
                    {
                        await ToastService.Show("启动浏览器失败，请重试");
                        Logger.Error("打开链接失败。", ex);
                    }
                });
            }
            catch (Exception ex)
            {
                if (!isBackgroundCheck)
                {
                    await ToastService.Show($"检查失败：{ex.Message}");
                }
                else
                {
                    Logger.Error("自动更新检查失败", ex);
                }
            }
        });
    }

}