using ListenTogether.HandCursorControls;

namespace ListenTogether.Pages;

public partial class SongMenuPage : ContentPage
{
    private SongMenuPageViewModel ViewModel => BindingContext as SongMenuPageViewModel;
    public SongMenuPage(SongMenuPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        HandCursor.Binding();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
        await ViewModel.InitializeAsync();
    }
    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
}