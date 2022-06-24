using JiuLing.CommonLibs.ExtensionMethods;
using ListenTogether.Data;
using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Business;

public class BusinessConfig
{
    /// <summary>
    /// 程序网络版本类型
    /// </summary>
    internal static AppNetworkEnum AppNetwork = AppNetworkEnum.Standalone;

    /// <summary>
    /// 用于认证的Token 信息
    /// </summary>
    public static TokenInfo? UserToken
    {
        set => DataConfig.UserToken = value;
    }

    /// <summary>
    /// 更新Token
    /// </summary>
    public static event EventHandler<TokenInfo?>? TokenUpdated;

    public static void SetDataBaseConnection(string path)
    {
        if (path.IsEmpty())
        {
            throw new ArgumentException("本地数据库配置参数错误");
        }
        DataConfig.SetDataBaseConnection(path);
    }
    public static void SetWebApi(string apiBaseUrl, string deviceId)
    {
        if (apiBaseUrl.IsEmpty() || deviceId.IsEmpty())
        {
            throw new ArgumentException("Web API配置参数错误");
        }
        DataConfig.TokenUpdated += TokenUpdated;
        DataConfig.SetWebApi(apiBaseUrl, deviceId);
        AppNetwork = AppNetworkEnum.Online;
    }
}