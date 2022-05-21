namespace ListenTogether.Pages;

public partial class LoginPage : ContentPage
{
    LoginPageViewModel vm => BindingContext as LoginPageViewModel;
    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        vm.InitializeAsync();
    }
}