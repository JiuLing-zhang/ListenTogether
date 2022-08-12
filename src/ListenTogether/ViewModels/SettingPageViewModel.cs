using ListenTogether.Model.Enums;

namespace ListenTogether.ViewModels;

public class SettingPageViewModel : ViewModelBase
{
    public ICommand SetApiDomainCommand => new Command(SetApiDomain);
    public ICommand OpenUrlCommand => new Command<string>(OpenUrl);
    public ICommand CheckUpdateCommand => new Command(CheckUpdate);
    public ICommand GoToCacheCleanCommand => new Command(GoToCacheClean);
    public ICommand GoToLogCommand => new Command(GoToLog);
    public ICommand GoToLoginCommand => new Command(GoToLogin);
    public ICommand LogoutCommand => new Command(Logout);

    private IEnvironmentConfigService _configService;
    private IUserService _userService;
    private IUserLocalService _userLocalService;
    public SettingPageViewModel(IEnvironmentConfigService configService, IUserService userService, IUserLocalService userLocalService)
    {
        _configService = configService;
        _userService = userService;
        _userLocalService = userLocalService;
    }
    public void InitializeAsync()
    {
        UserInfo = GetUserInfo();
        IsOnlineApp = GlobalConfig.AppNetwork == AppNetworkEnum.Online;
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

    private UserInfoViewModel _userInfo;

    /// <summary>
    /// 用户信息
    /// </summary>
    public UserInfoViewModel UserInfo
    {
        get => _userInfo;
        set
        {
            _userInfo = value;
            OnPropertyChanged();
        }
    }

    private bool _isOnlineApp;
    public bool IsOnlineApp
    {
        get => _isOnlineApp;
        set
        {
            _isOnlineApp = value;
            OnPropertyChanged();
        }
    }

    private string _loginUsername;
    public string LoginUsername
    {
        get => _loginUsername;
        set
        {
            _loginUsername = value;
            OnPropertyChanged();
        }
    }

    private string _loginPassword;
    public string LoginPassword
    {
        get => _loginPassword;
        set
        {
            _loginPassword = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 页面标题
    /// </summary>
    public string Title => "设置";

    private bool _isAutoCheckUpdate = GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate;
    /// <summary>
    /// 自动检查更新
    /// </summary>
    public bool IsAutoCheckUpdate
    {
        get => _isAutoCheckUpdate;
        set
        {
            _isAutoCheckUpdate = value;
            OnPropertyChanged();

            GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate = value;
            WriteGeneralConfigAsync();
        }
    }

    /// <summary>
    /// 深色主题
    /// </summary>
    public bool IsDarkMode
    {
        get => App.Current.UserAppTheme == AppTheme.Dark;
        set
        {
            App.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
            OnPropertyChanged();

            GlobalConfig.MyUserSetting.General.IsDarkMode = value;
            WriteGeneralConfigAsync();
        }
    }

    private string _apiDomain = GlobalConfig.ApiDomain;
    /// <summary>
    /// 服务器地址
    /// </summary>
    public string ApiDomain
    {
        get => _apiDomain.IsEmpty() ? "未设置" : _apiDomain;
        set
        {
            _apiDomain = value;
            OnPropertyChanged();
        }
    }

    private bool _isEnableNetEase = CheckEnablePlatform(PlatformEnum.NetEase);
    /// <summary>
    /// 网易云
    /// </summary>
    public bool IsEnableNetEase
    {
        get => _isEnableNetEase;
        set
        {
            _isEnableNetEase = value;
            OnPropertyChanged();

            Task.Run(async () =>
            {
                await EnablePlatformAsync(PlatformEnum.NetEase, value);
            });
        }
    }

    private bool _isEnableKuWo = CheckEnablePlatform(PlatformEnum.KuWo);
    /// <summary>
    /// 酷我
    /// </summary>
    public bool IsEnableKuWo
    {
        get => _isEnableKuWo;
        set
        {
            _isEnableKuWo = value;
            OnPropertyChanged();

            Task.Run(async () =>
            {
                await EnablePlatformAsync(PlatformEnum.KuWo, value);
            });
        }
    }

    private bool _isEnableKuGou = CheckEnablePlatform(PlatformEnum.KuGou);
    /// <summary>
    /// 酷狗
    /// </summary>
    public bool IsEnableKuGou
    {
        get => _isEnableKuGou;
        set
        {
            _isEnableKuGou = value;
            OnPropertyChanged();

            Task.Run(async () =>
            {
                await EnablePlatformAsync(PlatformEnum.KuGou, value);
            });
        }
    }

    private bool _isEnableMiGu = CheckEnablePlatform(PlatformEnum.MiGu);
    /// <summary>
    /// 咪咕
    /// </summary>
    public bool IsEnableMiGu
    {
        get => _isEnableMiGu;
        set
        {
            _isEnableMiGu = value;
            OnPropertyChanged();

            Task.Run(async () =>
            {
                await EnablePlatformAsync(PlatformEnum.MiGu, value);
            });
        }
    }

    private bool _isHideShortMusic = GlobalConfig.MyUserSetting.Search.IsHideShortMusic;
    /// <summary>
    /// 隐藏小于1分钟的歌曲
    /// </summary>
    public bool IsHideShortMusic
    {
        get => _isHideShortMusic;
        set
        {
            _isHideShortMusic = value;
            OnPropertyChanged();

            GlobalConfig.MyUserSetting.Search.IsHideShortMusic = value;
            WriteSearchConfigAsync();
        }
    }

    private bool _isPlayWhenAddToFavorite = GlobalConfig.MyUserSetting.Search.IsPlayWhenAddToFavorite;
    /// <summary>
    /// 添加到歌单时自动播放
    /// </summary>
    public bool IsPlayWhenAddToFavorite
    {
        get => _isPlayWhenAddToFavorite;
        set
        {
            _isPlayWhenAddToFavorite = value;
            OnPropertyChanged();

            GlobalConfig.MyUserSetting.Search.IsPlayWhenAddToFavorite = value;
            WriteSearchConfigAsync();
        }
    }

    private bool _isWifiPlayOnly = GlobalConfig.MyUserSetting.Play.IsWifiPlayOnly;
    /// <summary>
    /// 仅WIFI下可播放
    /// </summary>
    public bool IsWifiPlayOnly
    {
        get => _isWifiPlayOnly;
        set
        {
            _isWifiPlayOnly = value;
            OnPropertyChanged();

            GlobalConfig.MyUserSetting.Play.IsWifiPlayOnly = value;
            WritePlayConfigAsync();
        }
    }

    private bool _isAutoNextWhenFailed = GlobalConfig.MyUserSetting.Play.IsAutoNextWhenFailed;
    /// <summary>
    /// 播放失败时自动跳到下一首
    /// </summary>
    public bool IsAutoNextWhenFailed
    {
        get => _isAutoNextWhenFailed;
        set
        {
            _isAutoNextWhenFailed = value;
            OnPropertyChanged();

            GlobalConfig.MyUserSetting.Play.IsAutoNextWhenFailed = value;
            WritePlayConfigAsync();
        }
    }

    private bool _isCleanPlaylistWhenPlayMyFavorite = GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlayMyFavorite;
    /// <summary>
    /// 播放我的歌单前清空播放列表
    /// </summary>
    public bool IsCleanPlaylistWhenPlayMyFavorite
    {
        get => _isCleanPlaylistWhenPlayMyFavorite;
        set
        {
            _isCleanPlaylistWhenPlayMyFavorite = value;
            OnPropertyChanged();

            GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlayMyFavorite = value;
            WritePlayConfigAsync();
        }
    }

    private string _versionString = GlobalConfig.CurrentVersionString;
    /// <summary>
    /// 版本号
    /// </summary>
    public string VersionString
    {
        get => _versionString;
        set
        {
            _versionString = value;
            OnPropertyChanged();
        }
    }
    private static bool CheckEnablePlatform(PlatformEnum platform)
    {
        if ((GlobalConfig.MyUserSetting.Search.EnablePlatform & platform) == platform)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 保存通用配置
    /// </summary>
    private async Task WriteGeneralConfigAsync()
    {
        await _configService.WriteGeneralSettingAsync(GlobalConfig.MyUserSetting.General);
    }

    /// <summary>
    /// 保存播放配置
    /// </summary>
    private async Task WritePlayConfigAsync()
    {
        await _configService.WritePlaySettingAsync(GlobalConfig.MyUserSetting.Play);
    }

    private static UserInfoViewModel GetUserInfo()
    {
        if (GlobalConfig.CurrentUser == null)
        {
            return null;
        }
        return new UserInfoViewModel()
        {
            Username = GlobalConfig.CurrentUser.Username,
            Nickname = GlobalConfig.CurrentUser.Nickname,
            Avatar = $"{GlobalConfig.ApiDomain}{GlobalConfig.CurrentUser.Avatar}"
        };
    }

    private async Task EnablePlatformAsync(PlatformEnum platform, bool isEnable)
    {
        if (isEnable)
        {
            if (!CheckEnablePlatform(platform))
            {
                GlobalConfig.MyUserSetting.Search.EnablePlatform = GlobalConfig.MyUserSetting.Search.EnablePlatform | platform;
            }
        }
        else
        {
            if (CheckEnablePlatform(platform))
            {
                GlobalConfig.MyUserSetting.Search.EnablePlatform = GlobalConfig.MyUserSetting.Search.EnablePlatform & ~platform;
            }
        }
        await WriteSearchConfigAsync();
    }


    private async Task WriteSearchConfigAsync()
    {
        await _configService.WriteSearchSettingAsync(GlobalConfig.MyUserSetting.Search);
    }

    private async void SetApiDomain()
    {
        string result = await App.Current.MainPage.DisplayPromptAsync("服务器地址", "请输入要设置的地址（重启后生效）");
        if (result == null)
        {
            return;
        }
        ApiDomain = result;
        Preferences.Set("ApiDomain", result);
    }

    private async void GoToCacheClean()
    {
        await Shell.Current.GoToAsync($"{nameof(CacheCleanPage)}", true);
    }

    private async void GoToLog()
    {
        await Shell.Current.GoToAsync($"{nameof(LogPage)}", true);
    }

    private async void GoToLogin()
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}", true);
    }

    private async void Logout()
    {
        var isOk = await Shell.Current.DisplayAlert("提示", "确定要退出吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        try
        {
            IsBusy = true;

            //服务端退出失败时不处理，直接本地清除登录信息
            await _userService.Logout();

            _userLocalService.Remove();
            GlobalConfig.CurrentUser = null;
            UserInfo = null;
        }
        catch (Exception ex)
        {
            await ToastService.Show("退出失败，网络出小差了");
            Logger.Error("退出失败。", ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OpenUrl(string url)
    {
        Task.Run(async () =>
        {
            try
            {
                await Browser.Default.OpenAsync(url.ToUri(), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                await ToastService.Show("启动浏览器失败，请重试");
                Logger.Error("打开链接失败。", ex);
            }
        });
    }

    private async void CheckUpdate()
    {
        if (IsBusy == true)
        {
            return;
        }
        try
        {
            IsBusy = true;
            await UpdateCheck.Do(false);
        }
        catch (Exception ex)
        {
            await ToastService.Show("检查失败，网络出小差了");
            Logger.Error("自动更新检查失败。", ex);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
