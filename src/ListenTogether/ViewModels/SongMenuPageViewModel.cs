using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Filters.MusicSearchFilter;
using ListenTogether.Model.Enums;
using NetMusicLib.Models;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(Json), nameof(Json))]
[QueryProperty(nameof(PlatformString), nameof(PlatformString))]
public partial class SongMenuPageViewModel : ViewModelBase
{
    private readonly ILogger<SongMenuPageViewModel> _logger;
    public string Json { get; set; }
    public string PlatformString { get; set; }
    private PlatformEnum Platform => (PlatformEnum)Enum.Parse(typeof(PlatformEnum), PlatformString);

    private readonly IPlaylistService _playlistService;
    private readonly MusicNetPlatform _musicNetworkService;
    private readonly MusicResultService _musicResultService;

    [ObservableProperty]
    private SongMenuViewModel _songMenu;

    [ObservableProperty]
    private ObservableCollection<MusicResultGroupViewModel> _musicResultCollection = null!;

    public SongMenuPageViewModel(MusicNetPlatform musicNetworkService, MusicResultService musicResultService, IPlaylistService playlistService, ILogger<SongMenuPageViewModel> logger)
    {
        MusicResultCollection = new ObservableCollection<MusicResultGroupViewModel>();
        _musicNetworkService = musicNetworkService;
        _musicResultService = musicResultService;
        _playlistService = playlistService;
        _logger = logger;
    }
    public async Task InitializeAsync()
    {
        try
        {
            Loading("歌曲加载中....");
            MusicResultCollection.Clear();
            SongMenu = Json.ToObject<SongMenuViewModel>() ?? throw new ArgumentNullException("歌单信息不存在");

            List<Music> musics;
            switch (SongMenu.SongMenuType)
            {
                case SongMenuEnum.Tag:
                    musics = await _musicNetworkService.GetTagMusicsAsync((NetMusicLib.Enums.PlatformEnum)Platform, SongMenu.Id);
                    break;
                case SongMenuEnum.Top:
                    musics = await _musicNetworkService.GetTopMusicsAsync((NetMusicLib.Enums.PlatformEnum)Platform, SongMenu.Id);
                    break;
                default:
                    throw new ArgumentNullException("不支持的歌单类型");
            }

            IMusicSearchFilter vipMusicFilter = new VipMusicFilter();
            musics = vipMusicFilter.Filter(musics);

            int seq = 0;
            var platformMusics = musics.Select(
                  x => new MusicResultShowViewModel()
                  {
                      Seq = ++seq,
                      Id = x.Id,
                      Platform = (Model.Enums.PlatformEnum)x.Platform,
                      IdOnPlatform = x.IdOnPlatform,
                      Name = x.Name,
                      Artist = x.Artist,
                      Album = x.Album,
                      Duration = x.DurationText,
                      ImageUrl = x.ImageUrl,
                      Fee = x.Fee.GetDescription()
                  }).ToList();
            MusicResultCollection.Add(new MusicResultGroupViewModel(Platform.GetDescription(), platformMusics));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"歌曲加载失败：{SongMenu.PlatformName},type={SongMenu.SongMenuType},id={SongMenu.Id}");
        }
        finally
        {
            LoadComplete();
        }
    }

    [RelayCommand]
    public async void PlayAllAsync()
    {
        if (MusicResultCollection.Count == 0)
        {
            return;
        }
        if (GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlaySongMenu)
        {
            if (!await _playlistService.RemoveAllAsync())
            {
                await ToastService.Show("播放列表清空失败");
                return;
            }
        }
        await _musicResultService.PlayAllAsync(MusicResultCollection[0].ToLocalMusics());
    }

    [RelayCommand]
    public async void PlayAsync(MusicResultShowViewModel musicResult)
    {
        await _musicResultService.PlayAsync(musicResult.ToLocalMusic());
    }
}