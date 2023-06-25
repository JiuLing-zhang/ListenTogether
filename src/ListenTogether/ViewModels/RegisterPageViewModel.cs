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
    [Required(ErrorMessage = "请填写注册码")]
    private string _key = null!;

    [ObservableProperty]
    [Required(ErrorMessage = "请填写用户名")]
    private string _username = null!;

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


    [RelayCommand]
    private async void RegisterAsync()
    {
        try
        {
            Loading("正在注册....");

            ValidateAllProperties();
            if (HasErrors)
            {
                await ToastService.Show(GetErrors().First().ErrorMessage ?? "");
                return;
            }

            var user = new UserRegister()
            {
                Username = Username,
                Password = Password,
                Key = Key
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
            LoadComplete();
        }
    }
}