namespace MusicPlayerOnline.Maui;

public partial class MyFavoriteAddPage : ContentPage
{
    MyFavoriteAddPageViewModel vm => BindingContext as MyFavoriteAddPageViewModel;
    public MyFavoriteAddPage(MyFavoriteAddPageViewModel vm)
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