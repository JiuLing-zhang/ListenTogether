using ListenTogether.Model.Enums;

namespace ListenTogether.Pages;

public partial class MiGuPage : ContentPage
{
    DiscoverViewModel vm => BindingContext as DiscoverViewModel;
    public MiGuPage(DiscoverViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await vm.InitializeAsync(PlatformEnum.MiGu);
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

}