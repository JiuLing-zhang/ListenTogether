using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;

public class MusicResultGroupViewModel : List<MusicResultShowViewModel>
{
    public string Name { get; private set; }

    public MusicResultGroupViewModel(string name, List<MusicResultShowViewModel> musics) : base(musics)
    {
        Name = name;
    }
}
public partial class MusicResultShowViewModel : ObservableObject
{
    /// <summary>
    /// 平台名称
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

    /// <summary>
    /// 时长
    /// </summary>
    [ObservableProperty]
    private string _duration = null!;

    /// <summary>
    /// 费用（免费、VIP等）
    /// </summary>
    [ObservableProperty]
    private string _fee = null!;

}