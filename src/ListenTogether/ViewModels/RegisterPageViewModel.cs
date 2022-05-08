namespace ListenTogether.ViewModels;

public class RegisterPageViewModel : ViewModelBase
{
    public ICommand RegisterCommand => new Command(Register);
    public ICommand ChoseImageCommand => new Command(ChoseImage);

    private IUserService _userService;
    public RegisterPageViewModel(IUserService userService)
    {
        _userService = userService;
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

    private async void Register()
    {
        try
        {
            IsBusy = true;
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

            if (_userAvatar == null)
            {
                ApiMessage = "请选择头像";
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
            ApiMessage = message;
        }
        catch (Exception ex)
        {
            await ToastService.Show("注册失败，网络出小差了");
            Logger.Error("注册失败。", ex);
        }
        finally
        {
            IsBusy = false;
        }
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
            IsBusy = true;
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
                await ToastService.Show("仅支持jpg和png格式");
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
            await ToastService.Show("头像加载失败，请重试");
            Logger.Error("头像加载失败。", ex);
        }
        finally
        {
            IsBusy = false;
        }
    }
}