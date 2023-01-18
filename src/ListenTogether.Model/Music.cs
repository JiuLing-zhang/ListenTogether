using ListenTogether.Model.Enums;

namespace ListenTogether.Model;

public class Music : MusicBase
{
    /// <summary>
    /// 平台
    /// </summary>
    public PlatformEnum Platform { get; set; }

    /// <summary>
    /// 对应平台的ID
    /// </summary>
    public string PlatformInnerId { get; set; } = null!;
    /// <summary>
    /// 扩展数据
    /// </summary>
    public string ExtendData { get; set; } = null!;

}