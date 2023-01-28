using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Filters.MusicSearchFilter;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(Keyword), nameof(Keyword))]
public partial class SearchResultPageViewModel : ViewModelBase
{
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
    private ObservableCollection<MusicResultGroupViewModel> _musicSearchResult = null!;

    private int _isSearching = 0;
    private string _lastSearchKey;

    private readonly MusicResultService _musicResultService;
    private readonly IMusicNetworkService _musicNetworkService;

    public SearchResultPageViewModel(IMusicNetworkService musicNetworkService, MusicResultService musicResultService)
    {
        MusicSearchResult = new ObservableCollection<MusicResultGroupViewModel>();
        _musicNetworkService = musicNetworkService;
        _musicResultService = musicResultService;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

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
            if (_lastSearchKey == keyword)
            {
                return;
            }

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
                        .Select(x => new MusicResultShowViewModel()
                        {
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

                    MusicSearchResult.Add(new MusicResultGroupViewModel(platformName, onePlatformMusics));
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
            _lastSearchKey = keyword;
            StopLoading();
            Interlocked.Exchange(ref _isSearching, 0);
        }
    }

    [RelayCommand]
    public async void PlayAsync(MusicResultShowViewModel musicResult)
    {
        await _musicResultService.PlayAsync(musicResult.ToLocalMusic());
    }

    [RelayCommand]
    public async void AddToFavoriteAsync(MusicResultShowViewModel musicResult)
    {
        await _musicResultService.AddToFavoriteAsync(musicResult.ToLocalMusic());
    }

    [RelayCommand]
    private async void GoToSearchPageAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(SearchPage)}?Keyword={Keyword}", true);
        Keyword = "";
    }
}