using MusicPlayerOnline.Maui.ViewModels;

namespace MusicPlayerOnline.Maui.Pages;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchResultPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}