using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Filters.MusicSearchFilter;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public partial class SearchResultPageViewModel : ViewModelBase
{
    /// <summary>
    /// 搜索关键字
    /// </summary>
    [ObservableProperty]
    private string _keyword = null!;

    /// <summary>
    /// 搜索到的结果列表
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<MusicResultGroupViewModel> _searchResult = null!;

    private int _isSearching = 0;

    private readonly MusicResultService _musicResultService;
    private readonly IMusicNetworkService _musicNetworkService;
    private readonly IPlaylistService _playlistService;

    public SearchResultPageViewModel(IMusicNetworkService musicNetworkService, MusicResultService musicResultService, IPlaylistService playlistService)
    {
        SearchResult = new ObservableCollection<MusicResultGroupViewModel>();
        _musicNetworkService = musicNetworkService;
        _musicResultService = musicResultService;
        _playlistService = playlistService;
    }

    public async Task InitializeAsync()
    {
        await SearchAsync();
    }
    private async Task SearchAsync()
    {
        if (Keyword.IsEmpty())
        {
            return;
        }
        if (Interlocked.CompareExchange(ref _isSearching, 1, 0) != 0)
        {
            return;
        }

        try
        {
            Loading("正在搜索....");
            SearchResult.Clear();
            OnPropertyChanged("SearchResult");
            var musics = await _musicNetworkService.SearchAsync(GlobalConfig.MyUserSetting.Search.EnablePlatform, Keyword);

            if (GlobalConfig.MyUserSetting.Search.IsMatchSearchKey)
            {
                IMusicSearchFilter searchKeyFilter = new SearchKeyFilter(Keyword);
                musics = searchKeyFilter.Filter(musics);
            }
            if (GlobalConfig.MyUserSetting.Search.IsHideShortMusic)
            {
                IMusicSearchFilter shortMusicFilter = new ShortMusicFilter();
                musics = shortMusicFilter.Filter(musics);
            }

            IMusicSearchFilter vipMusicFilter = new VipMusicFilter();
            musics = vipMusicFilter.Filter(musics);

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
                    int seq = 0;
                    var onePlatformMusics = musics.Where(x => x.Platform == platform)
                        .Select(x => new MusicResultShowViewModel()
                        {
                            Seq = ++seq,
                            Id = x.Id,
                            IdOnPlatform = x.IdOnPlatform,
                            Platform = x.Platform,
                            Name = x.Name,
                            Artist = x.Artist,
                            Album = x.Album,
                            Duration = x.DurationText,
                            Fee = x.Fee.GetDescription(),
                            ImageUrl = x.ImageUrl,
                            ExtendDataJson = x.ExtendDataJson
                        }).ToList();

                    SearchResult.Add(new MusicResultGroupViewModel(platformName, onePlatformMusics));
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
            LoadComplete();
            Interlocked.Exchange(ref _isSearching, 0);
            OnPropertyChanged("SearchResult");
        }
    }

    [RelayCommand]
    public async void PlayAllAsync()
    {
        if (SearchResult.Count == 0)
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
        var musics = new List<LocalMusic>();
        for (int i = 0; i < SearchResult.Count; i++)
        {
            musics.AddRange(SearchResult[i].ToLocalMusics());
        }
        await _musicResultService.PlayAllAsync(musics);
    }

    [RelayCommand]
    public async void PlayAsync(MusicResultShowViewModel musicResult)
    {
        await _musicResultService.PlayAsync(musicResult.ToLocalMusic());
    }
}