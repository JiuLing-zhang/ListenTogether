using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ListenTogether.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    private IUserService _userService;
    private IUserLocalService _userLocalService;
    public LoginPageViewModel(IUserService userService, IUserLocalService userLocalService)
    {
        _userService = userService;
        _userLocalService = userLocalService;
    }
    public void InitializeAsync()
    {
        Username = "";
        Password = "";
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;
    public bool IsNotBusy => !_isBusy;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [RelayCommand]
    private async void Login()
    {
        try
        {
            IsBusy = true;
            if (Username.IsEmpty() || Password.IsEmpty())
            {
                await ToastService.Show("请输入用户名和密码");
                return;
            }

            var user = await _userService.Login(Username, Password);
            if (user == null)
            {
                await ToastService.Show("登录失败：用户名或密码错误");
                return;
            }

            if (!_userLocalService.Write(user))
            {
                await ToastService.Show("用户信息保存失败，请重试");
                return;
            }

            GlobalConfig.CurrentUser = user;
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
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async void GoToRegister()
    {
        await Shell.Current.GoToAsync($"../{nameof(RegisterPage)}", true);
    }
}