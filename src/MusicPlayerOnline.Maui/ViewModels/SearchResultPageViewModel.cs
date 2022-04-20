using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Enums;
using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels;

public class SearchResultPageViewModel : ViewModelBase
{
    private IServiceProvider _services;
    private readonly IMusicNetworkService _searchService;
    private IMusicService _musicService;
    private IPlaylistService _playlistService;
    private readonly PlayerService _playerService;

    private string _lastSearchKeyword = "";
    public ICommand AddToMyFavoriteCommand => new Command<SearchResultViewModel>(AddToMyFavorite);
    public ICommand PlayMusicCommand => new Command<SearchResultViewModel>(PlayMusic);
    public ICommand SearchCommand => new Command(Search);

    public SearchResultPageViewModel(IServiceProvider services, PlayerService playerService, IMusicNetworkService searchService)
    {
        MusicSearchResult = new ObservableCollection<SearchResultViewModel>();

        _services = services;
        _searchService = searchService;
        _playerService = playerService;
    }

    public async Task InitializeAsync()
    {
        _playlistService = _services.GetService<IPlaylistServiceFactory>().Create();
        _musicService = _services.GetService<IMusicServiceFactory>().Create();
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged("IsBusy");
            OnPropertyChanged("IsNotBusy");
        }
    }
    public bool IsNotBusy => !_isBusy;


    private string _textToSearch;
    /// <summary>
    /// 搜索
    /// </summary>
    public string TextToSearch
    {
        get => _textToSearch;
        set
        {
            _textToSearch = value;
            OnPropertyChanged();
        }
    }


    public IEnumerable<PlatformEnum> MyEnumTypeValues
    {
        get
        {
            return System.Enum.GetValues(typeof(PlatformEnum)).Cast<PlatformEnum>();
        }
    }

    private string _searchKeyword;
    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            _searchKeyword = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<SearchResultViewModel> _musicSearchResult;
    /// <summary>
    /// 搜索到的结果列表
    /// </summary>
    public ObservableCollection<SearchResultViewModel> MusicSearchResult
    {
        get => _musicSearchResult;
        set
        {
            _musicSearchResult = value;
            OnPropertyChanged();
        }
    }

    private SearchResultViewModel _musicSelectedResult;
    /// <summary>
    /// 选择的结果集
    /// </summary>
    public SearchResultViewModel MusicSelectedResult
    {
        get => _musicSelectedResult;
        set
        {
            _musicSelectedResult = value;
            OnPropertyChanged();
        }
    }
    private async void Search()
    {
        if (SearchKeyword.IsEmpty())
        {
            return;
        }

        if (SearchKeyword == _lastSearchKeyword)
        {
            return;
        }
        _lastSearchKeyword = SearchKeyword;

        try
        {
            IsBusy = true;
            MusicSearchResult.Clear();
            var musics = await _searchService.Search(GlobalConfig.MyUserSetting.Search.EnablePlatform, SearchKeyword);
            if (musics.Count == 0)
            {
                return;
            }

            foreach (var musicInfo in musics)
            {
                if (GlobalConfig.MyUserSetting.Search.IsHideShortMusic && musicInfo.Duration != 0 && musicInfo.Duration <= 60 * 1000)
                {
                    continue;
                }

                MusicSearchResult.Add(new SearchResultViewModel()
                {
                    Platform = musicInfo.Platform.GetDescription(),
                    Name = musicInfo.Name,
                    Alias = musicInfo.Alias == "" ? "" : $"（{musicInfo.Alias}）",
                    Artist = musicInfo.Artist,
                    Album = musicInfo.Album,
                    Duration = musicInfo.DurationText,
                    SourceData = musicInfo
                });
            }
        }
        catch (Exception ex)
        {
            await ToastService.Show("抱歉，网络可能出小差了~");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void AddToMyFavorite(SearchResultViewModel searchResult)
    {
        Music music;

        bool succeed;
        string message;
        (succeed, message, music) = await SaveMusic(searchResult.SourceData);
        if (succeed == false)
        {
            await ToastService.Show(message);
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(AddToMyFavoritePage)}?{nameof(AddToMyFavoritePageViewModel.AddedMusicId)}={music.Id}", true);
        await _playerService.PlayAsync(music);
    }

    private async void PlayMusic(SearchResultViewModel selected)
    {
        Music music;
        bool succeed;
        string message;

        (succeed, message, music) = await SaveMusic(selected.SourceData);
        if (succeed == false)
        {
            await ToastService.Show(message);
            return;
        }
        await _playerService.PlayAsync(music);
    }

    private async Task<(bool Succeed, string Message, Music MusicDetailResult)> SaveMusic(MusicSearchResult searchResult)
    {
        var music = await _searchService.GetMusicDetail(searchResult);
        if (music == null)
        {
            return (false, "emm没有解析出歌曲信息", null);
        }

        if (!await _musicService.AddOrUpdateAsync(music))
        {
            return (false, "保存歌曲信息失败", null);
        }

        var playlist = new Playlist()
        {
            MusicId = music.Id,
            MusicName = music.Name,
            MusicArtist = music.Artist
        };
        await _playlistService.AddToPlaylist(playlist);

        return (true, "", music);
    }
}