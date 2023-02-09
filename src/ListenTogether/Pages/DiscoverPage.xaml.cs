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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_isFirstAppearing)
        {
            _isFirstAppearing = false;
            HandCursor.Binding();
#if DEBUG == false
            await vm.InitializeAsync();
#endif

            if (GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate)
            {
                //确保主线程加载完成
                await Task.Delay(5000);

                //自动更新
                await UpdateCheck.Do(true);
            }
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}