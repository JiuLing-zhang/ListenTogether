using MusicPlayerOnline.Maui.ViewModels;

namespace MusicPlayerOnline.Maui.Pages;

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
        vm.ScrollLyric += ShowLyric;
        await vm.InitializeAsync();
    }
    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        vm.ScrollLyric -= ShowLyric;
        base.OnDisappearing();
    }

    private void ShowLyric(LyricViewModel lyricItem)
    {
        try
        {
            ListViewLyrics.ScrollTo(lyricItem, ScrollToPosition.Center, true);
        }
        catch (Exception ex)
        {

            Logger.Error("歌词同步展示失败。", ex);
        }
    }
}