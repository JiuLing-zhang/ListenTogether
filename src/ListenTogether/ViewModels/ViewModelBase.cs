using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class ViewModelBase : ObservableValidator
{
    private string _loadingKey = "";

    [ObservableProperty]
    private bool _isLogin;
    public ViewModelBase()
    {

    }

    internal void Loading(string message)
    {
        //同一页面不允许同时 Loading
        if (_loadingKey.IsNotEmpty())
        {
            return;
        }
        _loadingKey = JiuLing.CommonLibs.GuidUtils.GetFormatN();
        LoadingService.Loading(_loadingKey, message);
    }
    internal void LoadComplete()
    {
        if (_loadingKey.IsEmpty())
        {
            return;
        }
        LoadingService.LoadComplete(_loadingKey);
        _loadingKey = "";
    }
}
