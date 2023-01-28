using ListenTogether.Model.Enums;

namespace ListenTogether.Model;
public class MusicBase
{
    /// <summary>
    /// 平台
    /// </summary>
    public PlatformEnum Platform { get; set; }

    /// <summary>
    /// 对应平台的ID
    /// </summary>
    public string IdOnPlatform { get; set; } = null!;

    /// <summary>
    /// 歌曲名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 歌手名称
    /// </summary>
    public string Artist { get; set; } = null!;
    /// <summary>
    /// 专辑名称
    /// </summary>
    public string Album { get; set; } = null!;
    /// <summary>
    /// 图片地址
    /// </summary>
    public string ImageUrl { get; set; } = null!;

    /// <summary>
    /// 歌曲时长
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// 歌曲时长，格式为“分:秒”，例如：05:44
    /// </summary>
    public string DurationText => $"{Duration.Minutes}:{Duration.Seconds:D2}";

    /// <summary>
    /// 平台特有数据
    /// </summary>
    public string? ExtendDataJson { get; set; }
    /// <summary>
    /// 费用（免费、VIP等）
    /// </summary>
    public FeeEnum Fee { get; set; }
}