namespace MusicPlayerOnline.Data;
//TODO 文件重命名
public class DataConfig
{
    internal static readonly HttpClient HttpClientWithNoToken = new();
    internal static readonly HttpClient HttpClientWithToken = new(new ApiHttpMessageHandler());

    /// <summary>
    /// API 的一些配置信息
    /// </summary>
    internal static ApiSettings ApiSetting { get; set; } = null!;

    public static void SetDataConnection(string localDbPath, string apiBaseUrl, string deviceId)
    {
        DatabaseProvide.SetConnection(localDbPath);
        ApiSetting = new ApiSettings(apiBaseUrl, deviceId);
    }
}