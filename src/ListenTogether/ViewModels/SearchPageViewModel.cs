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

    public async Task InitializeAsync()
    {
        var a = await _musicNetworkService.GetHotWord();        
    }

    public async void GetSearchSuggest(TextChangedEventArgs e)
    {
        string keyword = e.NewTextValue;
        SearchSuggest.Clear();
        if (keyword.IsEmpty())
        {
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