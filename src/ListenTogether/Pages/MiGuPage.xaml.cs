namespace ListenTogether.Pages;

public partial class MiGuPage : ContentPage
{
    MiGuPageViewModel vm => BindingContext as MiGuPageViewModel;
    public MiGuPage(MiGuPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await vm.InitializeAsync();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}