using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Model.Enums;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(Json), nameof(Json))]
[QueryProperty(nameof(PlatformString), nameof(PlatformString))]
public partial class SongMenuPageViewModel : ViewModelBase
{
    public string Json { get; set; }
    public string PlatformString { get; set; }
    private PlatformEnum Platform => (PlatformEnum)Enum.Parse(typeof(PlatformEnum), PlatformString);

    private readonly IMusicNetworkService _musicNetworkService;
    private readonly IPlaylistService _playlistService = null!;
    private readonly MusicPlayerService _musicPlayerService;

    [ObservableProperty]
    private SongMenuViewModel _songMenu;

    [ObservableProperty]
    private ObservableCollection<MusicResultGroupViewModel> _musicResultCollection = null!;

    public SongMenuPageViewModel(IMusicNetworkService musicNetworkService, IPlaylistService playlistService, MusicPlayerService musicPlayerService)
    {
        MusicResultCollection = new ObservableCollection<MusicResultGroupViewModel>();
        _musicNetworkService = musicNetworkService;
        _playlistService = playlistService;
        _musicPlayerService = musicPlayerService;
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
                  Id = x.Id,
                  Platform = x.Platform,
                  IdOnPlatform = x.PlatformInnerId,
                  Name = x.Name,
                  Artist = x.Artist,
                  Album = x.Album,
                  Duration = x.DurationText,
                  ImageUrl = x.ImageUrl,
                  Fee = x.Fee.GetDescription()
              }).ToList();
        MusicResultCollection.Add(new MusicResultGroupViewModel(Platform.GetDescription(), platformMusics));
    }

    [RelayCommand]
    public async void PlayAsync(MusicResultShowViewModel musicResult)
    {
        await AddToPlaylistAsync(musicResult);
        await PlayMusicAsync(musicResult);
    }

    private async Task AddToPlaylistAsync(MusicResultShowViewModel musicResult)
    {
        var playlist = new Playlist()
        {
            MusicId = musicResult.Id,
            MusicIdOnPlatform = musicResult.IdOnPlatform,
            Platform = musicResult.Platform,
            MusicName = musicResult.Name,
            MusicArtist = musicResult.Artist,
            MusicAlbum = musicResult.Album,
            MusicImageUrl = musicResult.ImageUrl,
            EditTime = DateTime.Now
        };
        await _playlistService.AddToPlaylistAsync(playlist);
    }

    private async Task PlayMusicAsync(MusicResultShowViewModel musicResult)
    {
        await _musicPlayerService.PlayAsync(musicResult.Id);
    }

    [RelayCommand]
    public async void AddToFavoriteAsync(MusicResultShowViewModel id)
    {
        throw new NotImplementedException("添加到收藏");
    }
}