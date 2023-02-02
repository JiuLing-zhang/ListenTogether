using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Storage;

namespace ListenTogether.ViewModels;

public partial class LoginPageViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    public LoginPageViewModel(IUserService userService)
    {
        _userService = userService;
    }
    public Task InitializeAsync()
    {
        Username = "";
        Password = "";
        return Task.CompletedTask;
    }

    [ObservableProperty]
    private string _username = null!;

    [ObservableProperty]
    private string _password = null!;

    [RelayCommand]
    private async void LoginAsync()
    {
        try
        {
            Loading("正在登录....");
            if (Username.IsEmpty() || Password.IsEmpty())
            {
                await ToastService.Show("请输入用户名和密码");
                return;
            }

            var user = await _userService.LoginAsync(Username, Password);
            if (user == null)
            {
                await ToastService.Show("登录失败：用户名或密码错误");
                return;
            }

            UserInfoStorage.SetUsername(user.Username);
            UserInfoStorage.SetNickname(user.Nickname);
            UserInfoStorage.SetAvatar(user.Avatar);
            UserInfoStorage.SetToken(user.Token);
            UserInfoStorage.SetRefreshToken(user.RefreshToken);

            Username = "";
            Password = "";

            await Shell.Current.GoToAsync($"..", true);
        }
        catch (Exception ex)
        {
            await ToastService.Show("登录失败，网络出小差了");
            Logger.Error("登录失败。", ex);
        }
        finally
        {
            LoadComplete();
        }
    }

    [RelayCommand]
    private async void GoToRegisterAsync()
    {
        await Shell.Current.GoToAsync($"../{nameof(RegisterPage)}", true);
    }
}