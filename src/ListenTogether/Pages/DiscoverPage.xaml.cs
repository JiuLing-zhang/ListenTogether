using ListenTogether.Model.Enums;

namespace ListenTogether.Pages;

public partial class DiscoverPage : ContentPage
{
    DiscoverViewModel vm => BindingContext as DiscoverViewModel;
    public DiscoverPage(DiscoverViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        //Ŀǰ��ܲ�֧��shell�����Ĵ��Σ����ֻ��ͨ�����ַ�ʽ��ʱʵ��
        //https://github.com/dotnet/maui/issues/3868#issuecomment-1329418781
        vm.Platform = GetPlatformFromRoute();

        await vm.InitializeAsync();
        base.OnNavigatedTo(args);
    }
    private PlatformEnum GetPlatformFromRoute()
    {
        var route = Shell.Current.CurrentState.Location.OriginalString.Split("/").LastOrDefault() ?? "";
        return (PlatformEnum)Enum.Parse(typeof(PlatformEnum), route);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}