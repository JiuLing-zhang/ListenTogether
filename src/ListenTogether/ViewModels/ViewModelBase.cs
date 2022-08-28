using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class ViewModelBase : ObservableValidator
{
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
