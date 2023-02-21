using ListenTogether.Model.Enums;

namespace ListenTogether.Model;
/// <summary>
/// 搜索到的结果
/// </summary>
public class MusicSearchResult : MusicBase
{
    /// <summary>
    /// 对应平台的ID
    /// </summary>
    public string PlatformInnerId { get; set; } = null!;

    /// <summary>
    /// 平台特有数据
    /// </summary>
    public object PlatformData { get; set; } = null!;
}