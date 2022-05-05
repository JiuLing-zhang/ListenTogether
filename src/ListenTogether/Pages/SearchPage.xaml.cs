namespace ListenTogether.Pages;

public partial class SearchPage : ContentPage
{
    SearchResultPageViewModel vm => BindingContext as SearchResultPageViewModel;
    public SearchPage(SearchResultPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
        await vm.InitializeAsync();
    }

    private async void TxtSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        await vm.GetSearchSuggest(e.NewTextValue);
    }
}