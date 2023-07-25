using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Data.Api;

namespace ListenTogether.ViewModels;

public partial class LoginPageViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly ILoginDataStorage _loginDataStorage;
    private readonly ILogger<LoginPageViewModel> _logger;
    public LoginPageViewModel(IUserService userService, ILoginDataStorage loginDataStorage, ILogger<LoginPageViewModel> logger)
    {
        _userService = userService;
        _loginDataStorage = loginDataStorage;
        _logger = logger;
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

            _loginDataStorage.SetUsername(user.Username);
            _loginDataStorage.SetNickname(user.Nickname);
            _loginDataStorage.SetAvatar(user.Avatar);
            _loginDataStorage.SetToken(user.Token);
            _loginDataStorage.SetRefreshToken(user.RefreshToken);

            Username = "";
            Password = "";

            await Shell.Current.GoToAsync($"..", true);
        }
        catch (Exception ex)
        {
            await ToastService.Show("登录失败，网络出小差了");
            _logger.LogError(ex, "登录失败。");
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