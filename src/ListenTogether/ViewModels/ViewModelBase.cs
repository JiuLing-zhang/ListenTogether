using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class ViewModelBase : ObservableValidator
{
    public ViewModelBase()
    {
        MessagingCenter.Instance.Subscribe<PlayerService, bool>(this, "Player buffering", (sender, isBuffering) =>
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
    private string _loadingText;

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
}
