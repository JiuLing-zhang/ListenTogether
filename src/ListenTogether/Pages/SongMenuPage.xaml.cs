namespace ListenTogether.Pages;

public partial class SongMenuPage : ContentPage
{
    private SongMenuPageViewModel ViewModel => BindingContext as SongMenuPageViewModel;
    public SongMenuPage(SongMenuPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}