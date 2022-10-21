using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(Keyword), nameof(Keyword))]
public partial class SearchPageViewModel : ObservableObject
{
    private readonly IMusicNetworkService _musicNetworkService = null!;

    public SearchPageViewModel(IMusicNetworkService musicNetworkService)
    {
        SearchSuggest = new ObservableCollection<string>();
        _musicNetworkService = musicNetworkService;
    }

    /// <summary>
    /// 搜索关键字
    /// </summary>
    [ObservableProperty]
    private string _keyword = null!;

    private string _lastSearchKey = "";

    /// <summary>
    /// 搜索建议
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _searchSuggest = null!;

    private List<string> _hotWords = new List<string>();
    public async Task InitializeAsync()
    {
        _hotWords = await _musicNetworkService.GetHotWordAsync();
        await GetSearchSuggestAsync(Keyword);
    }

    public async Task GetSearchSuggestAsync(string keyword)
    {
        keyword = keyword.Trim();
        if (_lastSearchKey == keyword && keyword.IsNotEmpty())
        {
            return;
        }
        _lastSearchKey = keyword;

        SearchSuggest.Clear();
        if (keyword.IsEmpty())
        {
            foreach (var hotWord in _hotWords)
            {
                SearchSuggest.Add(hotWord);
            }
            return;
        }

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
    private async void BeginSearchAsync(string keyword)
    {
        await Shell.Current.GoToAsync($"..?Keyword={keyword}", true);
    }
}