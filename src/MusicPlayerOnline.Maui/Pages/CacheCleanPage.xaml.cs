namespace MusicPlayerOnline.Maui.Pages;

public partial class CacheCleanPage : ContentPage
{
    CacheCleanViewModel vm => BindingContext as CacheCleanViewModel;
    public CacheCleanPage(CacheCleanViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await vm.InitializeAsync();
    }
}