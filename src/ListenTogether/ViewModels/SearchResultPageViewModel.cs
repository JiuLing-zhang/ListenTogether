using ListenTogether.Model.Enums;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public class SearchResultPageViewModel : ViewModelBase
{
    private IServiceProvider _services;
    private readonly IMusicNetworkService _musicNetworkService;
    private IMusicService _musicService;
    private IPlaylistService _playlistService;
    private IMyFavoriteService _myFavoriteService;
    private readonly PlayerService _playerService;

    public ICommand AddToMyFavoriteCommand => new Command<SearchResultViewModel>(AddToMyFavorite);
    public ICommand PlayMusicCommand => new Command<SearchResultViewModel>(PlayMusic);
    public ICommand SearchCommand => new Command<string>(Search);
    public ICommand SuggestCommand => new Command<string>(Suggest);
    public ICommand SearchBarTextChangedCommand => new Command<TextChangedEventArgs>(GetSearchSuggest);

    public SearchResultPageViewModel(IServiceProvider services, PlayerService playerService, IMusicNetworkService musicNetworkService)
    {
        SearchSuggest = new ObservableCollection<string>();
        MusicSearchResult = new ObservableCollection<SearchResultViewModel>();
        MusicSearchResult.CollectionChanged += MusicSearchResult_CollectionChanged;
        _services = services;
        _musicNetworkService = musicNetworkService;
        _playerService = playerService;
    }

    private void MusicSearchResult_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged("MusicSearchResult");
    }

    public async Task InitializeAsync()
    {
        _playlistService = _services.GetService<IPlaylistServiceFactory>().Create();
        _musicService = _services.GetService<IMusicServiceFactory>().Create();
        _myFavoriteService = _services.GetService<IMyFavoriteServiceFactory>().Create();
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


    private string _keyword;
    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string Keyword
    {
        get => _keyword;
        set
        {
            _keyword = value;
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

    private ObservableCollection<string> _searchSuggest;
    /// <summary>
    /// 搜索建议
    /// </summary>
    public ObservableCollection<string> SearchSuggest
    {
        get => _searchSuggest;
        set
        {
            _searchSuggest = value;
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

    private bool _isDoSuggest = true;
    public async void GetSearchSuggest(TextChangedEventArgs e)
    {
        string keyword = e.NewTextValue;
        if (_isDoSuggest == false)
        {
            SearchSuggest.Clear();
            _isDoSuggest = true;
            return;
        }

        SearchSuggest.Clear();
        MusicSearchResult.Clear();

        if (keyword.IsEmpty())
        {
            OnPropertyChanged("SearchSuggest");
            return;
        }

        var suggests = await _musicNetworkService.GetSearchSuggest(keyword);
        if (suggests == null)
        {
            OnPropertyChanged("SearchSuggest");
            return;
        }
        foreach (var suggest in suggests)
        {
            SearchSuggest.Add(suggest);
        }
        OnPropertyChanged("SearchSuggest");
    }

    private void Suggest(string keyword)
    {
        _isDoSuggest = false;
        Keyword = keyword;
        Search(keyword);
    }

    private async void Search(string keyword)
    {
        SearchSuggest.Clear();

        if (keyword.IsEmpty())
        {
            return;
        }

        try
        {
            IsBusy = true;
            MusicSearchResult.Clear();
            var musics = await _musicNetworkService.Search(GlobalConfig.MyUserSetting.Search.EnablePlatform, keyword);
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
                    Fee = musicInfo.Fee.GetDescription(),
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
        try
        {
            IsBusy = true;

            Music music;

            bool succeed;
            string message;
            (succeed, message, music) = await SaveMusic(searchResult.SourceData);
            if (succeed == false)
            {
                await ToastService.Show(message);
                return;
            }

            //构造待选择的歌单项
            string[] myFavoriteButtons = null;
            var myFavoriteList = await _myFavoriteService.GetAllAsync();
            if (myFavoriteList != null)
            {
                myFavoriteButtons = myFavoriteList.Select(x => x.Name).ToArray();
            }

            int selectedMyFavoriteId;
            string myFavoriteItem = await App.Current.MainPage.DisplayActionSheet("请选择要加入的歌单", "取消", "创建一个新歌单", myFavoriteButtons);
            if (myFavoriteItem.IsEmpty() || myFavoriteItem == "取消")
            {
                return;
            }

            if (myFavoriteItem != "创建一个新歌单")
            {
                //使用已有歌单
                selectedMyFavoriteId = myFavoriteList.First(x => x.Name == myFavoriteItem).Id;
            }
            else
            {
                //新增歌单
                string myFavoriteName = await App.Current.MainPage.DisplayPromptAsync("添加歌单", "请输入歌单名称：", "添加", "取消");
                if (myFavoriteName.IsEmpty())
                {
                    await ToastService.Show("操作取消");
                    return;
                }

                if (await _myFavoriteService.NameExist(myFavoriteName))
                {
                    await ToastService.Show("歌单名称已存在");
                    return;
                }

                var myFavorite = new MyFavorite()
                {
                    Name = myFavoriteName,
                    MusicCount = 0
                };
                var newMyFavorite = await _myFavoriteService.AddOrUpdateAsync(myFavorite);
                if (newMyFavorite == null)
                {
                    await ToastService.Show("添加失败");
                    return;
                }
                selectedMyFavoriteId = newMyFavorite.Id;
            }

            var result = await _myFavoriteService.AddMusicToMyFavorite(selectedMyFavoriteId, music);
            if (result == false)
            {
                await ToastService.Show("添加失败");
                return;
            }
            if (GlobalConfig.MyUserSetting.Search.IsPlayWhenAddToFavorite)
            {
                await _playerService.PlayAsync(music);
            }
            else
            {
                await ToastService.Show("添加成功");
            }
        }
        catch (Exception ex)
        {
            await ToastService.Show("添加失败，网络出小差了");
            Logger.Error("搜索结果添加到我的歌单失败。", ex);
        }
        finally
        {
            IsBusy = false;
        }
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
        var music = await _musicNetworkService.GetMusicDetail(searchResult);
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
            PlatformName = music.PlatformName,
            MusicId = music.Id,
            MusicName = music.Name,
            MusicArtist = music.Artist,
            MusicAlbum = music.Album
        };
        await _playlistService.AddToPlaylist(playlist);

        return (true, "", music);
    }
}