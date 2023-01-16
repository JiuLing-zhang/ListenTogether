using CommunityToolkit.Mvvm.ComponentModel;
using ListenTogether.Model.Enums;
using System.Collections.ObjectModel;
using System.Web;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(Json), nameof(Json))]
[QueryProperty(nameof(PlatformString), nameof(PlatformString))]
public partial class SongMenuPageViewModel : ViewModelBase
{
    public string Json { get; set; }
    public string PlatformString { get; set; }
    private PlatformEnum Platform => (PlatformEnum)Enum.Parse(typeof(PlatformEnum), PlatformString);

    private readonly IMusicNetworkService _musicNetworkService;

    [ObservableProperty]
    private SongMenuViewModel _songMenu;

    [ObservableProperty]
    private ObservableCollection<MusicResultGroupViewModel> _musicResultCollection = null!;

    public SongMenuPageViewModel(IMusicNetworkService musicNetworkService)
    {
        _musicNetworkService = musicNetworkService;
        MusicResultCollection = new ObservableCollection<MusicResultGroupViewModel>();
    }
    public async Task InitializeAsync()
    {
        MusicResultCollection.Clear();
        SongMenu = Json.ToObject<SongMenuViewModel>() ?? throw new ArgumentNullException("歌单信息不存在");

        List<MusicResultShow> musics;
        switch (SongMenu.SongMenuType)
        {
            case SongMenuEnum.Tag:
                musics = await _musicNetworkService.GetTagMusicsAsync(Platform, SongMenu.Id);
                break;
            case SongMenuEnum.Top:
                musics = await _musicNetworkService.GetTopMusicsAsync(Platform, SongMenu.Id);
                break;
            default:
                throw new ArgumentNullException("不支持的歌单类型");
        }

        var platformMusics = musics.Select(
              x => new MusicResultShowViewModel()
              {
                  Platform = x.Platform.GetDescription(),
                  Name = x.Name,
                  Artist = x.Artist,
                  Album = x.Album,
                  Duration = x.DurationText,
                  Fee = x.Fee.GetDescription()
              }).ToList();
        MusicResultCollection.Add(new MusicResultGroupViewModel(Platform.GetDescription(), platformMusics));
    }
}