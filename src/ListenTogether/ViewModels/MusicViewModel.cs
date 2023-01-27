using CommunityToolkit.Mvvm.ComponentModel;
using ListenTogether.Model.Enums;

namespace ListenTogether.ViewModels;
public partial class MusicViewModel : ObservableObject
{
    /// <summary>
    /// 歌曲Id
    /// </summary>
    [ObservableProperty]
    private string _id = null!;

    /// <summary>
    /// 平台Id
    /// </summary>
    [ObservableProperty]
    private string _idOnPlatform = null!;

    /// <summary>
    /// 平台
    /// </summary>
    [ObservableProperty]
    private PlatformEnum _platform;

    /// <summary>
    /// 平台名称
    /// </summary>
    [ObservableProperty]
    private string _platformName;

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

    /// <summary>
    /// 图片
    /// </summary>
    [ObservableProperty]
    private string _imageUrl = null!;

}
