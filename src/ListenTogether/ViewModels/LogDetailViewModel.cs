using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;

public partial class LogDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private string _time = null!;

    [ObservableProperty]
    private string _message = null!;
}