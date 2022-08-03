namespace ListenTogether.Pages;

public partial class SettingsPage : ContentPage
{
    SettingPageViewModel vm => BindingContext as SettingPageViewModel;
    public SettingsPage(SettingPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
        vm.InitializeAsync();
    }

    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
}