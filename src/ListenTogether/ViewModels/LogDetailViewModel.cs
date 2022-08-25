using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;

public partial class LogDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private string _time;

    [ObservableProperty]
    private string _message;
}