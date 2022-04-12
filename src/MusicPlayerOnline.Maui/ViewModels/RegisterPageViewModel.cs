namespace MusicPlayerOnline.Maui.ViewModels;

public class RegisterPageViewModel : ViewModelBase
{
    public ICommand GoBackCommand => new Command(GoBack);
    public ICommand RegisterCommand => new Command(Register);
    public ICommand ChoseImageCommand => new Command(ChoseImage);


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

    private bool _isShowRegister = true;
    public bool IsShowRegister
    {
        get => _isShowRegister;
        set
        {
            _isShowRegister = value;
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
        IsShowRegister = false;
        if (Username.IsEmpty() || Nickname.IsEmpty() || Password.IsEmpty())
        {
            ApiMessage = "注册信息不完整";
            IsShowRegister = true;
            return;
        }
        if (Password != Password2)
        {
            ApiMessage = "两次密码不一致";
            IsShowRegister = true;
            return;
        }

        if (_userAvatar == null)
        {
            ApiMessage = "请选择头像";
            IsShowRegister = true;
            return;
        }

        var user = new UserRegister()
        {
            Username = Username,
            Nickname = Nickname,
            Password = Password,
            Avatar = _userAvatar
        };

        var (succeed, message) = await _userService.Register(user);
        IsShowRegister = !succeed;
        ApiMessage = message;
    }

    private ImageSource _myImage;
    public ImageSource MyImage
    {
        get => _myImage;
        set
        {
            _myImage = value;
            OnPropertyChanged();
        }
    }

    private UserAvatar _userAvatar;

    private async void ChoseImage()
    {
        try
        {
            _userAvatar = null;
            MyImage = null;

            var result = await FilePicker.PickAsync(new PickOptions());
            if (result == null)
            {
                return;
            }

            if (!result.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
                !result.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _userAvatar = new UserAvatar();
            _userAvatar.FileName = result.FileName;

            var stream = await result.OpenReadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                _userAvatar.File = ms.ToArray();
            }

            MyImage = ImageSource.FromFile(result.FullPath);

        }
        catch (Exception ex)
        {
            ToastService.Show($"头像加载失败：{ex.Message}");
        }
    }

}