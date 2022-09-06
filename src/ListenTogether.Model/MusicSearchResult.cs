using ListenTogether.Model.Enums;

namespace ListenTogether.Model;
/// <summary>
/// 搜索到的结果
/// </summary>
public class MusicSearchResult : MusicBase
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
    /// 平台特有数据
    /// </summary>
    public object PlatformData { get; set; } = null!;
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