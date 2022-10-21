namespace ListenTogether.Pages;

public partial class SearchPage : ContentPage
{
    SearchPageViewModel vm => BindingContext as SearchPageViewModel;
    public SearchPage(SearchPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await vm.InitializeAsync();
        await Task.Delay(300);
        TxtSearchBar.Focus();
    }

    private async void TxtSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        await vm.GetSearchSuggestAsync(e?.NewTextValue);
    }
}