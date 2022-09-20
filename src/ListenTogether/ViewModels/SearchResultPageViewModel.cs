using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Filters.MusicSearchFilter;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(Keyword), nameof(Keyword))]
public partial class SearchResultPageViewModel : ViewModelBase
{
    private readonly PlayerService _playerService = null!;
    private readonly IServiceProvider _services = null!;
    private readonly IMusicNetworkService _musicNetworkService = null!;
    private readonly IPlaylistService _playlistService = null!;
    private IMusicService _musicService = null!;
    private IMyFavoriteService _myFavoriteService = null!;

    public SearchResultPageViewModel(IServiceProvider services, IPlaylistService playlistService, PlayerService playerService, IMusicNetworkService musicNetworkService)
    {
        MusicSearchResult = new ObservableCollection<SearchResultGroupViewModel>();
        _services = services;
        _musicNetworkService = musicNetworkService;
        _playerService = playerService;
        _playlistService = playlistService;
    }

    public Task InitializeAsync()
    {
        _musicService = _services.GetRequiredService<IMusicServiceFactory>().Create();
        _myFavoriteService = _services.GetRequiredService<IMyFavoriteServiceFactory>().Create();
        return Task.CompletedTask;
    }

    /// <summary>
    /// 搜索关键字
    /// </summary>
    [ObservableProperty]
    private string _keyword = null!;
    partial void OnKeywordChanged(string value)
    {
        Task.Run(async () =>
        {
            await SearchAsync(value);
        });
    }

    /// <summary>
    /// 搜索到的结果列表
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<SearchResultGroupViewModel> _musicSearchResult = null!;

    /// <summary>
    /// 选择的结果集
    /// </summary>
    [ObservableProperty]
    private SearchResultViewModel _musicSelectedResult = null!;

    private int _isSearching = 0;
    private async Task SearchAsync(string keyword)
    {
        if (keyword.IsEmpty())
        {
            return;
        }
        if (Interlocked.CompareExchange(ref _isSearching, 1, 0) != 0)
        {
            return;
        }

        try
        {
            StartLoading("正在搜索....");
            MusicSearchResult.Clear();
            var musics = await _musicNetworkService.SearchAsync(GlobalConfig.MyUserSetting.Search.EnablePlatform, keyword);

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
            StopLoading();
            Interlocked.Exchange(ref _isSearching, 0);
        }
    }

    [RelayCommand]
    private async void AddToMyFavoriteAsync(SearchResultViewModel searchResult)
    {
        try
        {
            StartLoading("处理中....");

            var (succeed, message, music) = await SaveMusicAsync(searchResult.SourceData);
            if (succeed == false)
            {
                await ToastService.Show(message);
                return;
            }

            //构造待选择的歌单项
            string[]? myFavoriteButtons = null;
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

            if (myFavoriteItem != "创建一个新歌单" && myFavoriteList != null)
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

                if (await _myFavoriteService.NameExistAsync(myFavoriteName))
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

            var result = await _myFavoriteService.AddMusicToMyFavoriteAsync(selectedMyFavoriteId, music);
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
            StopLoading();
        }
    }

    [RelayCommand]
    private async void PlayMusicAsync(SearchResultViewModel selected)
    {
        var (succeed, message, music) = await SaveMusicAsync(selected.SourceData);
        if (succeed == false)
        {
            await ToastService.Show(message);
            return;
        }
        await _playerService.PlayAsync(music);
    }

    private async Task<(bool Succeed, string Message, Music MusicDetailResult)> SaveMusicAsync(MusicSearchResult searchResult)
    {
        var music = await _musicNetworkService.GetDetailAsync(searchResult, GlobalConfig.MyUserSetting.Play.MusicFormatType);
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
        await _playlistService.AddToPlaylistAsync(playlist);

        return (true, "", music);
    }

    [RelayCommand]
    private async void GoToSearchPageAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(SearchPage)}?Keyword={Keyword}", true);
        Keyword = "";
    }
}