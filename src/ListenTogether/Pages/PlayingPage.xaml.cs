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
        base.OnDisappearing();
    }

    private void ScrollToLyricDo(object sender, LyricViewModel e)
    {
        try
        {
            ListLyrics.ScrollTo(e, null, ScrollToPosition.Center, true);
        }
        catch (Exception ex)
        {
            Logger.Error("歌词同步展示失败。", ex);
        }
    }
}
