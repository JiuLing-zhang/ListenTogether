using CommunityToolkit.Mvvm.ComponentModel;
using ListenTogether.Model.Enums;

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

    public SongMenuPageViewModel(IMusicNetworkService musicNetworkService)
    {
        _musicNetworkService = musicNetworkService;

    }
    public async Task InitializeAsync()
    {
        SongMenu = Json.ToObject<SongMenuViewModel>() ?? throw new ArgumentNullException("歌单信息不存在");
        var musics = await _musicNetworkService.GetTopMusicsAsync(Platform, SongMenu.Id);
    }
}