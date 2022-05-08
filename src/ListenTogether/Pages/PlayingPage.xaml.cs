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

    //控制手动滚动歌词时，系统暂停歌词滚动
    private DateTime _lastScrollToTime = DateTime.Now;
    private void ScrollToLyricDo(object sender, LyricViewModel e)
    {
        try
        {
            if (_lastScrollToTime.Subtract(DateTime.Now).TotalMilliseconds > 0)
            {
                return;
            }
            ListLyrics.ScrollTo(e, null, ScrollToPosition.Center, true);
        }
        catch (Exception ex)
        {
            Logger.Error("歌词同步展示失败。", ex);
        }
    }

    private void ListLyrics_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        _lastScrollToTime = DateTime.Now.AddSeconds(1);
    }
}
