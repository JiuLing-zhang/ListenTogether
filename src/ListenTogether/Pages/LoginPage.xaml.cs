namespace ListenTogether.Pages;

public partial class LoginPage : ContentPage
{
    LoginPageViewModel vm => BindingContext as LoginPageViewModel;
    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await vm.InitializeAsync();
    }
}