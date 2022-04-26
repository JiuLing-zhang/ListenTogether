using ListenTogether.ViewModels;

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
        vm.ScrollToLyric += ScrollToLyric;
        await vm.InitializeAsync();
    }
    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        vm.ScrollToLyric -= ScrollToLyric;
        base.OnDisappearing();
    }

    private void ScrollToLyric(LyricViewModel lyric)
    {
        try
        {
            ListLyrics.ScrollTo(lyric);
        }
        catch (Exception ex)
        {
            Logger.Error("歌词同步展示失败。", ex);
        }
    }
}