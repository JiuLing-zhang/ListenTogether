using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(Json), nameof(Json))]
public partial class SongMenuPageViewModel : ViewModelBase
{
    public string Json { get; set; }
    private readonly IMusicNetworkService _musicNetworkService;

    [ObservableProperty]
    private SongMenuViewModel _songMenu;

    public SongMenuPageViewModel(IMusicNetworkService musicNetworkService)
    {
        _musicNetworkService = musicNetworkService;

    }
    public async Task InitializeAsync()
    {
        SongMenu = Json.ToObject<SongMenuViewModel>();
    }
}