namespace ListenTogether.Pages;

public partial class AutoClosePage : ContentPage
{
    AutoClosePageViewModel vm => BindingContext as AutoClosePageViewModel;
    public AutoClosePage(AutoClosePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await vm.Initialize();
    }

    protected override async void OnDisappearing()
    {
        await vm.Disappearing();
        base.OnDisappearing();
    }
}