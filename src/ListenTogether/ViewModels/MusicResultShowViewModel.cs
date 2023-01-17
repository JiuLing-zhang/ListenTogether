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
public partial class MusicResultShowViewModel : MusicViewModel
{
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