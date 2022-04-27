using ListenTogether.ViewModels;

namespace ListenTogether.Pages;

public partial class SettingsPage : ContentPage
{
    SettingPageViewModel vm => BindingContext as SettingPageViewModel;
    public SettingsPage(SettingPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
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