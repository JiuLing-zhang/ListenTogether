namespace ListenTogether.Pages;

public partial class SearchResultPage : ContentPage
{
    private bool _isFirstStart = true;
    SearchResultPageViewModel vm => BindingContext as SearchResultPageViewModel;
    public SearchResultPage(SearchResultPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
        await vm.InitializeAsync();
        if (GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate)
        {
            if (_isFirstStart)
            {
                _isFirstStart = false;

                if (GlobalConfig.ApiDomain.IsEmpty())
                {
                    await ToastService.Show("温馨提示：当前程序为【单机版】");
                }
                //确保主线程加载完成
                await Task.Delay(5000);
                await UpdateCheck.Do(true);
            }
        }
    }
    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
}