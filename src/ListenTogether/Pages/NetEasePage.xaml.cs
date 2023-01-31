using ListenTogether.HandCursorControls;
using ListenTogether.Model.Enums;

namespace ListenTogether.Pages;

public partial class NetEasePage : ContentPage
{
    DiscoverViewModel vm => BindingContext as DiscoverViewModel;
    public NetEasePage(DiscoverViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
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
        await vm.InitializeAsync(PlatformEnum.NetEase);
    }
    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
}