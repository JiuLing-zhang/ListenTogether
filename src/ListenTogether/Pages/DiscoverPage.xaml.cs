using ListenTogether.HandCursorControls;

namespace ListenTogether.Pages;

public partial class DiscoverPage : ContentPage
{
    private bool _isFirstAppearing = true;
    DiscoverPageViewModel vm => BindingContext as DiscoverPageViewModel;
    public DiscoverPage(DiscoverPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_isFirstAppearing)
        {
            _isFirstAppearing = false;
            HandCursor.Binding();
            await vm.InitializeAsync();
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}