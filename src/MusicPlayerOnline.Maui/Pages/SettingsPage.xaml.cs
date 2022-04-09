using MusicPlayerOnline.Maui.ViewModels;

namespace MusicPlayerOnline.Maui.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }

}