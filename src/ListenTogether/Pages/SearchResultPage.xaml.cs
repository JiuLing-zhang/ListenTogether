using ListenTogether.HandCursorControls;

namespace ListenTogether.Pages;

public partial class SearchResultPage : ContentPage
{
    SearchResultPageViewModel vm => BindingContext as SearchResultPageViewModel;
    public SearchResultPage(SearchResultPageViewModel vm)
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
        await vm.InitializeAsync();
    }
    public void SetKeyword(string keyword)
    {
        vm.Keyword = keyword;
    }

    private void TxtSearchBar_Focused(object sender, FocusEventArgs e)
    {
        Navigation.PopAsync(false);
    }
}