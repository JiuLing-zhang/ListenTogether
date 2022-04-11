namespace MusicPlayerOnline.Maui;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}