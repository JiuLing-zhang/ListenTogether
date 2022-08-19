using ListenTogether.Filters.MusicSearchFilter;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(Keyword), nameof(Keyword))]
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
    public ICommand GoToSearchPageCommand => new Command(GoToSearchPage);

    public SearchResultPageViewModel(IServiceProvider services, IPlaylistService playlistService, PlayerService playerService, IMusicNetworkService musicNetworkService)
    {
        MusicSearchResult = new ObservableCollection<SearchResultGroupViewModel>();
        MusicSearchResult.CollectionChanged += MusicSearchResult_CollectionChanged;
        _services = services;
        _musicNetworkService = musicNetworkService;
        _playerService = playerService;
        _playlistService = playlistService;
    }

    private void MusicSearchResult_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged("MusicSearchResult");
    }

    public async Task InitializeAsync()
    {
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
            Task.Run(async () =>
            {
                await Search(value);
            });
        }
    }

    private ObservableCollection<SearchResultGroupViewModel> _musicSearchResult;
    /// <summary>
    /// 搜索到的结果列表
    /// </summary>
    public ObservableCollection<SearchResultGroupViewModel> MusicSearchResult
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

    int running = 0;
    private async Task Search(string keyword)
    {
        if (keyword.IsEmpty())
        {
            return;
        }
        if (Interlocked.CompareExchange(ref running, 1, 0) != 0)
        {
            return;
        }

        try
        {
            IsBusy = true;
            MusicSearchResult.Clear();
            var musics = await _musicNetworkService.Search(GlobalConfig.MyUserSetting.Search.EnablePlatform, keyword);

            if (GlobalConfig.MyUserSetting.Search.IsMatchSearchKey)
            {
                IMusicSearchFilter filter = new SearchKeyFilter(keyword);
                musics = filter.Filter(musics);
            }
            if (GlobalConfig.MyUserSetting.Search.IsHideShortMusic)
            {
                IMusicSearchFilter filter = new ShortMusicFilter();
                musics = filter.Filter(musics);
            }
            if (GlobalConfig.MyUserSetting.Search.IsHideVipMusic)
            {
                IMusicSearchFilter filter = new VipMusicFilter();
                musics = filter.Filter(musics);
            }

            if (musics.Count == 0)
            {
                return;
            }

            var platformList = musics.Select(x => x.Platform).Distinct().ToList();
            foreach (var platform in platformList)
            {
                string platformName = platform.GetDescription();
                try
                {
                    var onePlatformMusics = musics.Where(x => x.Platform == platform)
                        .Select(x => new SearchResultViewModel()
                        {
                            Platform = x.Platform.GetDescription(),
                            Name = x.Name,
                            Alias = x.Alias == "" ? "" : $"（{x.Alias}）",
                            Artist = x.Artist,
                            Album = x.Album,
                            Duration = x.DurationText,
                            Fee = x.Fee.GetDescription(),
                            SourceData = x
                        }).ToList();

                    MusicSearchResult.Add(new SearchResultGroupViewModel(platformName, onePlatformMusics));
                }
                catch (Exception e)
                {
                    await ToastService.Show($"【{platformName}】搜索结果加载失败");
                    Logger.Error("搜索结果添加失败。", e);
                }
            }

        }
        catch (Exception ex)
        {
            await ToastService.Show("抱歉，网络可能出小差了~");
        }
        finally
        {
            IsBusy = false;
            Interlocked.Exchange(ref running, 0);
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
            if (GlobalConfig.MyUserSetting.Play.IsPlayWhenAddToFavorite)
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

    private async void GoToSearchPage()
    {
        await Shell.Current.GoToAsync($"{nameof(SearchPage)}?Keyword={Keyword}", true);
        Keyword = "";
    }
}