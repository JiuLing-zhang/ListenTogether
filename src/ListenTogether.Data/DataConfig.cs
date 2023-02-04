namespace ListenTogether.Data;

public class DataConfig
{
    internal static readonly HttpClient HttpClientWithNoToken = new()
    {
        Timeout = TimeSpan.FromSeconds(10)
    };
    internal static readonly HttpClient HttpClientWithToken = new(new ApiHttpMessageHandler())
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    /// <summary>
    /// API 的一些配置信息
    /// </summary>
    internal static ApiSettings ApiSetting { get; set; } = null!;
    public static void SetDataBaseConnection(string path)
    {
        DatabaseProvide.SetConnection(path);
    }

    public static void SetWebApi(string apiBaseUrl, string deviceId)
    {
        ApiSetting = new ApiSettings(apiBaseUrl, deviceId);
    }
}