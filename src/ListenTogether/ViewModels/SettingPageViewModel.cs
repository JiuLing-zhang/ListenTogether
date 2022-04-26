using ListenTogether.Model.Enums;

namespace ListenTogether.ViewModels;

public class SettingPageViewModel : ViewModelBase
{
    public ICommand OpenUrlCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
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

    public async Task InitializeAsync()
    {
        GetAppConfig();
        UpdateUserInfo();

        //TODO IAppVersionInfo
        //VersionName = DependencyService.Get<IAppVersionInfo>().GetVersionName();
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

    private bool _isAutoCheckUpdate;
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
            WriteGeneralConfig();
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
            WriteGeneralConfig();
        }
    }

    private bool _isEnableNetEase;
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

            EnableNetEase();
        }
    }

    private bool _isEnableKuGou;
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

            EnableKuGou();
        }
    }

    private bool _isEnableMiGu;
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

            EnableMiGu();
        }
    }

    private bool _isHideShortMusic;
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
            WriteSearchConfig();
        }
    }


    private bool _isWifiPlayOnly;
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
            WritePlayConfig();
        }
    }

    private bool _isAutoNextWhenFailed;
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
            WritePlayConfig();
        }
    }

    private bool _isCleanPlaylistWhenPlayMyFavorite;
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
            WritePlayConfig();
        }
    }

    private string _versionName;
    /// <summary>
    /// 版本号
    /// </summary>
    public string VersionName
    {
        get => _versionName;
        set
        {
            _versionName = value;
            OnPropertyChanged();
        }
    }

    private void GetAppConfig()
    {
        //常规设置
        IsAutoCheckUpdate = GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate;

        //搜索平台设置
        IsEnableNetEase = CheckEnablePlatform(PlatformEnum.NetEase);
        IsEnableKuGou = CheckEnablePlatform(PlatformEnum.KuGou);
        IsEnableMiGu = CheckEnablePlatform(PlatformEnum.MiGu);
        IsHideShortMusic = GlobalConfig.MyUserSetting.Search.IsHideShortMusic;

        //播放设置
        IsWifiPlayOnly = GlobalConfig.MyUserSetting.Play.IsWifiPlayOnly;
        IsAutoNextWhenFailed = GlobalConfig.MyUserSetting.Play.IsAutoNextWhenFailed;
        IsCleanPlaylistWhenPlayMyFavorite = GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlayMyFavorite;
    }

    private void UpdateUserInfo()
    {
        if (GlobalConfig.CurrentUser == null)
        {
            UserInfo = null;
        }
        else
        {
            UserInfo = new UserInfoViewModel()
            {
                Username = GlobalConfig.CurrentUser.Username,
                Nickname = GlobalConfig.CurrentUser.Nickname,
                Avatar = GlobalConfig.CurrentUser.Avatar
            };
        }
    }

    private bool CheckEnablePlatform(PlatformEnum platform)
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
    private async void WriteGeneralConfig()
    {
        await Task.Run(() =>
        {
            _configService.WriteGeneralSetting(GlobalConfig.MyUserSetting.General);
        });
    }

    /// <summary>
    /// 保存播放配置
    /// </summary>
    private async void WritePlayConfig()
    {
        await Task.Run(() =>
        {
            _configService.WritePlaySetting(GlobalConfig.MyUserSetting.Play);
        });
    }

    private async void EnableNetEase()
    {
        if (IsEnableNetEase)
        {
            if (!CheckEnablePlatform(PlatformEnum.NetEase))
            {
                GlobalConfig.MyUserSetting.Search.EnablePlatform = GlobalConfig.MyUserSetting.Search.EnablePlatform | PlatformEnum.NetEase;
            }
        }
        else
        {
            if (CheckEnablePlatform(PlatformEnum.NetEase))
            {
                GlobalConfig.MyUserSetting.Search.EnablePlatform = GlobalConfig.MyUserSetting.Search.EnablePlatform & ~PlatformEnum.NetEase;
            }
        }
        await WriteSearchConfig();
    }

    private async void EnableKuGou()
    {
        if (IsEnableKuGou)
        {
            if (!CheckEnablePlatform(PlatformEnum.KuGou))
            {
                GlobalConfig.MyUserSetting.Search.EnablePlatform = GlobalConfig.MyUserSetting.Search.EnablePlatform | PlatformEnum.KuGou;
            }
        }
        else
        {
            if (CheckEnablePlatform(PlatformEnum.KuGou))
            {
                GlobalConfig.MyUserSetting.Search.EnablePlatform = GlobalConfig.MyUserSetting.Search.EnablePlatform & ~PlatformEnum.KuGou;
            }
        }
        await WriteSearchConfig();
    }

    private async void EnableMiGu()
    {
        if (IsEnableMiGu)
        {
            if (!CheckEnablePlatform(PlatformEnum.MiGu))
            {
                GlobalConfig.MyUserSetting.Search.EnablePlatform = GlobalConfig.MyUserSetting.Search.EnablePlatform | PlatformEnum.MiGu;
            }
        }
        else
        {
            if (CheckEnablePlatform(PlatformEnum.MiGu))
            {
                GlobalConfig.MyUserSetting.Search.EnablePlatform = GlobalConfig.MyUserSetting.Search.EnablePlatform & ~PlatformEnum.MiGu;
            }
        }
        await WriteSearchConfig();
    }

    private async Task WriteSearchConfig()
    {
        await Task.Run(() =>
        {
            _configService.WriteSearchSetting(GlobalConfig.MyUserSetting.Search);
        });
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
            UpdateUserInfo();
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
}
