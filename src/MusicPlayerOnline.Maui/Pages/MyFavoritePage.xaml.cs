using MusicPlayerOnline.Maui.ViewModels;

namespace MusicPlayerOnline.Maui.Pages;

public partial class MyFavoritePage : ContentPage
{
    public MyFavoritePage(MyFavoritePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}