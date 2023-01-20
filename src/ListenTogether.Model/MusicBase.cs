using ListenTogether.Model.Enums;

namespace ListenTogether.Model;
public class MusicBase
{
    public string Id { get; set; } = null!;
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
}