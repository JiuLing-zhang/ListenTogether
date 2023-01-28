using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace ListenTogether.ViewModels;

public partial class RegisterPageViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    public RegisterPageViewModel(IUserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    [Required(ErrorMessage = "请填写用户名")]
    private string _username = null!;

    [ObservableProperty]
    [Required(ErrorMessage = "请填写昵称")]
    private string _nickname = null!;

    [ObservableProperty]
    [Required(ErrorMessage = "请填写密码")]
    private string _password = null!;


    private string _password2 = null!;
    [Required(ErrorMessage = "请填写确认密码")]
    [Compare(otherProperty: nameof(Password), ErrorMessage = "两次密码不一致")]
    public string Password2
    {
        get => _password2;
        set => SetProperty(ref _password2, value);
    }

    [ObservableProperty]
    private ImageSource? _myImage;

    private UserAvatar? _userAvatar;

    [RelayCommand]
    private async void RegisterAsync()
    {
        try
        {
            StartLoading("正在注册....");

            ValidateAllProperties();
            if (HasErrors)
            {
                await ToastService.Show(GetErrors().First().ErrorMessage ?? "");
                return;
            }

            if (_userAvatar == null)
            {
                await ToastService.Show("请选择头像");
                return;
            }

            if (_userAvatar.File.Length >= 500 * 1024)
            {
                await ToastService.Show("头像最大允许 500KB");
                return;
            }

            var aaa = _userAvatar.File.Length;
            var user = new UserRegister()
            {
                Username = Username,
                Nickname = Nickname,
                Password = Password,
                Avatar = _userAvatar
            };

            var (succeed, message) = await _userService.RegisterAsync(user);
            await ToastService.Show(message);
        }
        catch (Exception ex)
        {
            await ToastService.Show("注册失败，网络出小差了");
            Logger.Error("注册失败。", ex);
        }
        finally
        {
            StopLoading();
        }
    }


    [RelayCommand]
    private async void ChoseImageAsync()
    {
        try
        {
            _userAvatar = null;
            MyImage = null;

            PickOptions options = new()
            {
                PickerTitle = "请选择头像",
                FileTypes = FilePickerFileType.Images
            };

            var result = await FilePicker.PickAsync(options);
            if (result == null)
            {
                return;
            }
            StartLoading("");

            _userAvatar = new UserAvatar();
            _userAvatar.FileName = result.FileName;

            var stream = await result.OpenReadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
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
            StopLoading();
        }
    }
}