using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(Keyword), nameof(Keyword))]
public class SearchPageViewModel : ViewModelBase
{
    private readonly IMusicNetworkService _musicNetworkService;
    public ICommand BeginSearchCommand => new Command<string>(BeginSearch);
    public ICommand SearchBarTextChangedCommand => new Command<TextChangedEventArgs>(GetSearchSuggest);

    public SearchPageViewModel(IMusicNetworkService musicNetworkService)
    {
        SearchSuggest = new ObservableCollection<string>();
        _musicNetworkService = musicNetworkService;
    }

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

    private List<string> _hotWords = new List<string>();
    public async Task InitializeAsync()
    {
        _hotWords = await _musicNetworkService.GetHotWord();
        //TODO 目前搜索栏的TextChanged 事件有bug，暂时屏蔽搜索建议
        foreach (var hotWord in _hotWords)
        {
            SearchSuggest.Add(hotWord);
        }
        return;
        //TODO End
        await GetSearchSuggest(Keyword);
    }

    public async void GetSearchSuggest(TextChangedEventArgs e)
    {
        await GetSearchSuggest(e?.NewTextValue);
    }

    public async Task GetSearchSuggest(string keyword)
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

        var suggests = await _musicNetworkService.GetSearchSuggest(keyword);
        if (suggests == null)
        {
            return;
        }
        foreach (var suggest in suggests)
        {
            SearchSuggest.Add(suggest);
        }
    }

    private async void BeginSearch(string keyword)
    {
        await Shell.Current.GoToAsync($"..?Keyword={keyword}", true);
    }
}