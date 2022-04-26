namespace MusicPlayerOnline.Maui.Pages;

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

    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
}