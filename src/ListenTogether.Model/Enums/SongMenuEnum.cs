using System.ComponentModel;

namespace ListenTogether.Model.Enums;

/// <summary>
/// 歌单类型
/// </summary>
public enum SongMenuEnum
{
    /// <summary>
    /// 标签歌单
    /// </summary>
    [Description("标签歌单")]
    Tag,

    /// <summary>
    /// 排行榜歌单
    /// </summary>
    [Description("排行榜歌单")]
    Top,
}