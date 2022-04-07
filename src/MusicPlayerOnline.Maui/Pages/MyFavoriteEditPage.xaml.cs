namespace MusicPlayerOnline.Maui;

public partial class MyFavoriteEditPage : ContentPage
{
    MyFavoriteEditPageViewModel vm => BindingContext as MyFavoriteEditPageViewModel;
    public MyFavoriteEditPage(MyFavoriteEditPageViewModel vm)
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