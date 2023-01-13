namespace ListenTogether.Pages;

public partial class ChooseTagPage : ContentPage
{
    ChooseTagPageViewModel vm => BindingContext as ChooseTagPageViewModel;
    public ChooseTagPage(ChooseTagPageViewModel vm)
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