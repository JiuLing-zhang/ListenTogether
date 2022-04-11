namespace MusicPlayerOnline.Maui.ViewModels;

public class RegisterPageViewModel : ViewModelBase
{
    public ICommand GoBackCommand => new Command(GoBack);
    public ICommand RegisterCommand => new Command(Register);


    private IEnvironmentConfigService _configService;
    private IUserService _userService;
    private IUserLocalService _userLocalService;
    public RegisterPageViewModel(IEnvironmentConfigService configService, IUserService userService, IUserLocalService userLocalService)
    {
        _configService = configService;
        _userService = userService;
        _userLocalService = userLocalService;
    }

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

    private string _nickname;
    public string Nickname
    {
        get => _nickname;
        set
        {
            _nickname = value;
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

    private string _password2;
    public string Password2
    {
        get => _password2;
        set
        {
            _password2 = value;
            OnPropertyChanged();
        }
    }

    private string _apiMessage;
    public string ApiMessage
    {
        get => _apiMessage;
        set
        {
            _apiMessage = value;
            OnPropertyChanged();
        }
    }

    private bool _registerSucceed;
    public bool RegisterSucceed
    {
        get => _registerSucceed;
        set
        {
            _registerSucceed = value;
            OnPropertyChanged();
        }
    }

    private async void GoBack()
    {
        await Shell.Current.GoToAsync($"..", true);
    }
    private async void Register()
    {
        ApiMessage = "";
        if (Username.IsEmpty() || Nickname.IsEmpty() || Password.IsEmpty())
        {
            ApiMessage = "注册信息不完整";
            return;
        }
        if (Password != Password2)
        {
            ApiMessage = "两次密码不一致";
            return;
        }

        var user = new UserRegister()
        {
            Username = Username,
            Nickname = Nickname,
            Password = Password
        };

        var (succeed, message) = await _userService.Register(user);
        RegisterSucceed = succeed;
        ApiMessage = message;
    }
}