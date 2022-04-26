namespace ListenTogether;

internal class AppSettings
{
    /// <summary>
    /// 本地数据库名
    /// </summary>
    public string LocalDbName { get; set; } = null!;

    /// <summary>
    /// 设备信息文件名
    /// </summary>
    public string DeviceInfoFileName { get; set; } = null!;

    /// <summary>
    /// API 服务的根路径
    /// </summary>
    public string ApiDomain { get; set; }
}