using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
/// <summary>
/// 歌词详情
/// </summary>
public partial class LyricViewModel : ObservableObject
{
    /// <summary>
    /// 歌词位置
    /// </summary>
    [ObservableProperty]
    private int _positionMillisecond;

    /// <summary>
    /// 歌词
    /// </summary>
    [ObservableProperty]
    private string _info;

    /// <summary>
    /// 是否高亮显示
    /// </summary>
    [ObservableProperty]
    private bool _isHighlight = false;

}