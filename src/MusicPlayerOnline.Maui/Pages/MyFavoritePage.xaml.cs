namespace MusicPlayerOnline.Maui.Pages;

public partial class MyFavoritePage : ContentPage
{
    MyFavoritePageViewModel vm => BindingContext as MyFavoritePageViewModel;
    public MyFavoritePage(MyFavoritePageViewModel vm)
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