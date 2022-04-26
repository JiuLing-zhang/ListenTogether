using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Maui.ViewModels;

public class LoginPageViewModel : ViewModelBase
{
    public ICommand LoginCommand => new Command(Login);
    public ICommand GoToRegisterCommand => new Command(GoToRegister);
    public ICommand GoBackCommand => new Command(GoBack);

    private IUserService _userService;
    private IUserLocalService _userLocalService;
    public LoginPageViewModel(IUserService userService, IUserLocalService userLocalService)
    {
        _userService = userService;
        _userLocalService = userLocalService;
    }


    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged("IsBusy");
            OnPropertyChanged("IsNotBusy");
        }
    }
    public bool IsNotBusy => !_isBusy;

    private string _username;
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

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
            GoBack();
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
    private async void GoToRegister()
    {
        await Shell.Current.GoToAsync($"../{nameof(RegisterPage)}", true);
    }

    private async void GoBack()
    {
        await Shell.Current.GoToAsync($"..", true);
    }
}