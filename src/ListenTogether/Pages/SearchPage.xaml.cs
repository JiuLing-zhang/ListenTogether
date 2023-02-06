using ListenTogether.HandCursorControls;

namespace ListenTogether.Pages;

public partial class SearchPage : ContentPage
{
    SearchPageViewModel vm => BindingContext as SearchPageViewModel;
    public SearchPage(SearchPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        HandCursor.Binding();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(300);
        await vm.InitializeAsync();
        TxtSearchBar.Focus();
    }
}