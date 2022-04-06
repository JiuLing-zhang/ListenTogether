namespace MusicPlayerOnline.Maui;

public partial class MyFavoriteAddPage : ContentPage
{
    public MyFavoriteAddPage(MyFavoriteAddPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}