using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;

public partial class UserInfoViewModel : ObservableObject
{
    [ObservableProperty]
    private string _username = null!;

    [ObservableProperty]
    private string _nickname = null!;

    [ObservableProperty]
    private string _avatar = null!;
}