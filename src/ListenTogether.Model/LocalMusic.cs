using ListenTogether.Model.Enums;

namespace ListenTogether.Model;
public class LocalMusic : MusicBase
{
    /// <summary>
    /// 平台特有数据
    /// </summary>
    public string? ExtendDataJson { get; set; }
    /// <summary>
    /// 费用（免费、VIP等）
    /// </summary>
    public FeeEnum Fee { get; set; }

    /// <summary>
    /// 歌曲时长
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// 歌曲时长，格式为“分:秒”，例如：05:44
    /// </summary>
    public string DurationText => $"{Duration.Minutes}:{Duration.Seconds:D2}";
}