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
        get => DataConfig.UserToken;
        set => DataConfig.UserToken = value;
    }
    /// <summary>
    /// Token 已更新
    /// </summary>
    public static event EventHandler? TokenUpdated;

    public static void SetWebApi(string localDbPath, string apiBaseUrl, string deviceId)
    {
        DataConfig.TokenUpdated += TokenUpdated;
        DataConfig.SetDataConnection(localDbPath, apiBaseUrl, deviceId);
    }

    /// <summary>
    /// 是否使用 API 接口
    /// </summary>
    internal static bool IsUseApiInterface => UserToken != null;
}