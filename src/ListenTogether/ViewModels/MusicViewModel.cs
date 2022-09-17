using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class MusicViewModel : ObservableObject
{
    [ObservableProperty]
    private string _id = null!;

    /// <summary>
    /// 平台
    /// </summary>
    [ObservableProperty]
    private string _platform = null!;

    /// <summary>
    /// 歌曲名称
    /// </summary>
    [ObservableProperty]
    private string _name = null!;

    /// <summary>
    /// 歌手名称
    /// </summary>
    [ObservableProperty]
    private string _artist = null!;

    /// <summary>
    /// 专辑名称
    /// </summary>
    [ObservableProperty]
    private string _album = null!;
}
