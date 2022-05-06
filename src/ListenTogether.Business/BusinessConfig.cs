using ListenTogether.Data;
using ListenTogether.Model;

namespace ListenTogether.Business;

public class BusinessConfig
{
    /// <summary>
    /// 用于认证的Token 信息
    /// </summary>
    public static TokenInfo? UserToken
    {
        set => DataConfig.UserToken = value;
    }

    /// <summary>
    /// 是否使用 API 接口
    /// </summary>
    internal static bool IsUseApiInterface => DataConfig.UserToken != null;

    /// <summary>
    /// 更新Token
    /// </summary>
    public static event EventHandler<TokenInfo?>? TokenUpdated;

    public static void SetWebApi(string localDbPath, string apiBaseUrl, string deviceId)
    {
        DataConfig.TokenUpdated += TokenUpdated;
        DataConfig.SetDataConnection(localDbPath, apiBaseUrl, deviceId);
    }
}