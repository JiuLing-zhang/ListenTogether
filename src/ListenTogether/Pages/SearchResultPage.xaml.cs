namespace ListenTogether.Pages;

public partial class SearchResultPage : ContentPage
{
    SearchResultPageViewModel vm => BindingContext as SearchResultPageViewModel;
    public SearchResultPage(SearchResultPageViewModel vm)
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
}