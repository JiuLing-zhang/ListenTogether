using CommunityToolkit.Mvvm.ComponentModel;
using ListenTogether.Storage;

namespace ListenTogether.ViewModels;
public partial class ViewModelBase : ObservableValidator
{
    public ViewModelBase()
    {
        MessagingCenter.Instance.Subscribe<string, bool>(this, "PlayerBuffering", (sender, isBuffering) =>
        {
            if (isBuffering)
            {
                StartLoading("歌曲缓冲中....");
            }
            else
            {
                StopLoading();
            }
        });

        MessagingCenter.Instance.Subscribe<string>(this, "ClearUserInfo", async (sender) =>
        {
            await ToastService.Show("登录信息已过期，请重新登录");
        });

    }

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _loadingText = null!;

    internal void StartLoading(string loadingText)
    {
        IsLoading = true;
        LoadingText = loadingText;
    }

    internal void StopLoading()
    {
        IsLoading = false;
        LoadingText = "";
    }

    internal bool IsLogin => GlobalConfig.AppNetwork == Model.Enums.AppNetworkEnum.Standalone || UserInfoStorage.GetUsername().IsNotEmpty();
    internal bool IsNotLogin => !IsLogin;
}
