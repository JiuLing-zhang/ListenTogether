using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ListenTogether.Data.Api;

namespace ListenTogether.ViewModels;

public partial class SearchPageViewModel : ViewModelBase
{
    private readonly IMusicNetworkService _musicNetworkService = null!;
    private readonly SearchResultPage _searchResultPage;
    private readonly ISearchHistoryStorage _searchHistoryStorage;
    public SearchPageViewModel(IMusicNetworkService musicNetworkService, SearchResultPage searchResultPage, ISearchHistoryStorage searchHistoryStorage)
    {
        SearchHistories = new ObservableCollection<string>();
        HotWords = new ObservableCollection<string>();
        SearchSuggest = new ObservableCollection<string>();
        _musicNetworkService = musicNetworkService;
        _searchResultPage = searchResultPage;
        _searchHistoryStorage = searchHistoryStorage;
    }

    /// <summary>
    /// 搜索关键字
    /// </summary>
    [ObservableProperty]
    private string _keyword = null!;
    async partial void OnKeywordChanged(string value)
    {
        await GetSearchSuggestAsync(value);
    }

    private string _lastSearchKey = "";

    /// <summary>
    /// 历史搜索记录
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _searchHistories;

    /// <summary>
    /// 热门搜索
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _hotWords;

    /// <summary>
    /// 搜索建议
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _searchSuggest = null!;


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotSearchingForSuggest))]
    private bool _isSearchingForSuggest = false;
    public bool IsNotSearchingForSuggest => !IsSearchingForSuggest;

    public async Task InitializeAsync()
    {
        await GetGetHistoriesAsync();
        await GetHotWords();
#if WINDOWS
        //TODO 搜索栏有个BUG，如果初始化时指定Text，点击删除时则不会出发TextChanged事件
        //所以页面显示时，暂时屏蔽掉搜索建议栏的展示
        IsSearchingForSuggest = false;
#endif
    }
    private Task GetGetHistoriesAsync()
    {
        SearchHistories.Clear();
        var searchHistories = _searchHistoryStorage.GetHistories();
        foreach (var searchHistory in searchHistories)
        {
            SearchHistories.Add(searchHistory);
        }
        return Task.CompletedTask;
    }

    private async Task GetHotWords()
    {
        //热门搜索只获取一次
        if (HotWords.Count > 0)
        {
            return;
        }
        var hotWords = await _musicNetworkService.GetHotWordAsync();
        if (hotWords != null)
        {
            foreach (var hotWord in hotWords)
            {
                HotWords.Add(hotWord);
            }
        }
    }

    private async Task GetSearchSuggestAsync(string keyword)
    {
        keyword = keyword.Trim();

        if (_lastSearchKey == keyword)
        {
            return;
        }

        IsSearchingForSuggest = false;
        SearchSuggest.Clear();
        if (keyword.IsEmpty())
        {
            return;
        }
        _lastSearchKey = keyword;
        IsSearchingForSuggest = true;
        try
        {
            var suggests = await _musicNetworkService.GetSearchSuggestAsync(keyword);
            if (suggests == null)
            {
                return;
            }
            foreach (var suggest in suggests)
            {
                SearchSuggest.Add(suggest);
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"搜索建议加载失败：{keyword}", ex);
        }
    }

    [RelayCommand]
    private async void RemoveSearchHistoriesAsync()
    {
        var isOk = await Shell.Current.DisplayAlert("提示", "确定要删除全部搜索历史吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }
        _searchHistoryStorage.Clear();
        SearchHistories.Clear();
    }

    [RelayCommand]
    private async void BeginSearchAsync(string keyword)
    {
        await DoSearchAsync(keyword);
    }

    [RelayCommand]
    private async void HistoriyWordClickedAsync(string historiyWord)
    {
        await DoSearchAsync(historiyWord);
    }

    [RelayCommand]
    private async void HotWordClickedAsync(string hotWord)
    {
        await DoSearchAsync(hotWord);
    }
    private async Task DoSearchAsync(string keyword)
    {
        _searchHistoryStorage.Add(keyword);
        _searchResultPage.SetKeyword(keyword);
        await App.Current.MainPage.Navigation.PushAsync(_searchResultPage, false);
    }
}