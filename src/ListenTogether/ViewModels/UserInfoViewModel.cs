using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;

public partial class UserInfoViewModel : ObservableObject
{
    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _nickname;

    [ObservableProperty]
    private string _avatar;
}