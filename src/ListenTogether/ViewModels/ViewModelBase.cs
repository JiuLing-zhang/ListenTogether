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

    internal bool IsLogin => UserInfoStorage.GetUsername().IsNotEmpty();
    internal bool IsNotLogin => !IsLogin;
}
