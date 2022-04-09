namespace MusicPlayerOnline.Maui.Pages;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchResultPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
}