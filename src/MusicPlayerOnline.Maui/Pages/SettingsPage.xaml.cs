using MusicPlayerOnline.Maui.ViewModels;

namespace MusicPlayerOnline.Maui.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}