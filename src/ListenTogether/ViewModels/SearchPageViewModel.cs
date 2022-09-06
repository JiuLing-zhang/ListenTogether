using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(Keyword), nameof(Keyword))]
public partial class SearchPageViewModel : ObservableObject
{
    private readonly IMusicNetworkService _musicNetworkService;

    public SearchPageViewModel(IMusicNetworkService musicNetworkService)
    {
        SearchSuggest = new ObservableCollection<string>();
        _musicNetworkService = musicNetworkService;
    }

    /// <summary>
    /// 搜索关键字
    /// </summary>
    [ObservableProperty]
    private string _keyword;

    /// <summary>
    /// 搜索建议
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _searchSuggest;
      
    private List<string> _hotWords = new List<string>();
    public async Task InitializeAsync()
    {
        _hotWords = await _musicNetworkService.GetHotWordAsync();
        //TODO 目前搜索栏的TextChanged 事件有bug，暂时屏蔽搜索建议
        foreach (var hotWord in _hotWords)
        {
            SearchSuggest.Add(hotWord);
        }
        return;
        //TODO End
        await GetSearchSuggestAsync(Keyword);
    }

    [RelayCommand]
    public async void GetSearchSuggestAsync(TextChangedEventArgs e)
    {
        await GetSearchSuggestAsync(e?.NewTextValue);
    }

    public async Task GetSearchSuggestAsync(string keyword)
    {
        //TODO 目前搜索栏的TextChanged 事件有bug，暂时屏蔽搜索建议
        return;
        //TODO End
        SearchSuggest.Clear();
        if (keyword.IsEmpty())
        {
            foreach (var hotWord in _hotWords)
            {
                SearchSuggest.Add(hotWord);
            }
            return;
        }

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

    [RelayCommand]
    private async void BeginSearchAsync(string keyword)
    {
        await Shell.Current.GoToAsync($"..?Keyword={keyword}", true);
    }
}