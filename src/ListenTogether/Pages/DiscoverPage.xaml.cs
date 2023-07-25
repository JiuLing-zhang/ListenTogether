using ListenTogether.HandCursorControls;

namespace ListenTogether.Pages;

public partial class DiscoverPage : ContentPage
{
    private bool _isFirstAppearing = true;
    //控制每次滚动时，只加载一页数据
    private DateTime _lastScrollToTime = DateTime.Now;
    DiscoverPageViewModel vm => BindingContext as DiscoverPageViewModel;
    private readonly UpdateCheck _updateCheck;
    public DiscoverPage(DiscoverPageViewModel vm, UpdateCheck updateCheck)
    {
        InitializeComponent();
        BindingContext = vm;
        _updateCheck = updateCheck;
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
                await _updateCheck.DoAsync(true);
            }
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        //TODO WinUI 中，目前无法触发 RemainingItemsThresholdReachedCommand，因此先自己实现一下
        if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
        {
            return;
        }
        if (_lastScrollToTime.Subtract(DateTime.Now).TotalMilliseconds > 0)
        {
            return;
        }
        _lastScrollToTime = DateTime.Now.AddSeconds(1);

        if (sender is CollectionView cv && cv is IElementController element)
        {
            var count = element.LogicalChildren.Count;
            if (e.LastVisibleItemIndex + 1 - count + cv.RemainingItemsThreshold >= 0)
            {
                if (cv.RemainingItemsThresholdReachedCommand.CanExecute(null))
                {
                    cv.RemainingItemsThresholdReachedCommand.Execute(null);
                }
            }
        }
    }
}