using MusicPlayerOnline.Repository;

namespace MusicPlayerOnline.Service;
public class GlobalConfig
{

    /// <summary>
    /// API 的一些配置信息
    /// </summary>
    public static ApiSettings ApiSetting { get; set; } = null!;


    public static void SetDbConnection(string dbPath)
    {
        DatabaseProvide.SetConnection(dbPath);
    }

    public static void SetWebApi(string baseUrl, string deviceId)
    {
        ApiSetting = new ApiSettings(baseUrl, deviceId);
    }
    
    /// <summary>
    /// 是否使用 API 接口
    /// </summary>
    public static bool IsUseApiInterface { get; set; }
}