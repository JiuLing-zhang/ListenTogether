namespace MusicPlayerOnline.Maui;

public partial class LogPage : ContentPage
{
    LogPageViewModel vm => BindingContext as LogPageViewModel;
    public LogPage(LogPageViewModel vm)
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