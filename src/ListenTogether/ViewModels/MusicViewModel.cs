using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class MusicViewModel : ObservableObject
{
    [ObservableProperty]
    private string _id;

    /// <summary>
    /// 平台
    /// </summary>
    [ObservableProperty]
    private string _platform;

    /// <summary>
    /// 歌曲名称
    /// </summary>
    [ObservableProperty]
    private string _name;

    /// <summary>
    /// 歌手名称
    /// </summary>
    [ObservableProperty]
    private string _artist;

    /// <summary>
    /// 专辑名称
    /// </summary>
    [ObservableProperty]
    private string _album;
}
