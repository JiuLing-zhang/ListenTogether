using ListenTogether.HandCursorControls;

namespace ListenTogether.Pages;

public partial class MyFavoriteDetailPage : ContentPage
{
    MyFavoriteDetailPageViewModel vm => BindingContext as MyFavoriteDetailPageViewModel;
    public MyFavoriteDetailPage(MyFavoriteDetailPageViewModel vm)
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
        await vm.InitializeAsync();
    }
}