namespace ListenTogether.Pages;

public partial class PlayingPage : ContentPage
{
    PlayingPageViewModel vm => BindingContext as PlayingPageViewModel;
    public PlayingPage(PlayingPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
        vm.ScrollToLyric += ScrollToLyricDo;
        await vm.InitializeAsync();
    }
    protected override void OnDisappearing()
    {
        vm.ScrollToLyric -= ScrollToLyricDo;
        player.OnDisappearing();
        vm.OnDisappearing();
        base.OnDisappearing();
    }

    private void ScrollToLyricDo(object sender, LyricViewModel e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ListLyrics.ScrollTo(e, null, ScrollToPosition.Center, true);
        });
    }
}
