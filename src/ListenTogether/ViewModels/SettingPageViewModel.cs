using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Model.Enums;

namespace ListenTogether.ViewModels;

public partial class SettingPageViewModel : ObservableObject
{
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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;
    public bool IsNotBusy => !_isBusy;

    /// <summary>
    /// 用户信息
    /// </summary>
    [ObservableProperty]
    private UserInfoViewModel _userInfo;


    [ObservableProperty]
    private bool _isOnlineApp;

    [ObservableProperty]
    private string _loginUsername;

    [ObservableProperty]
    private string _loginPassword;


    /// <summary>
    /// 页面标题
    /// </summary>
    public string Title => "设置";

    /// <summary>
    /// 自动检查更新
    /// </summary>
    [ObservableProperty]
    private bool _isAutoCheckUpdate = GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate;
    partial void OnIsAutoCheckUpdateChanged(bool value)
    {
        GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate = value;
        WriteGeneralConfigAsync();
    }

    /// <summary>
    /// 深色主题
    /// </summary>
    [ObservableProperty]
    private bool _isDarkMode = App.Current.UserAppTheme == AppTheme.Dark;
    partial void OnIsDarkModeChanged(bool value)
    {
        App.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
        GlobalConfig.MyUserSetting.General.IsDarkMode = value;
        WriteGeneralConfigAsync();
    }

    /// <summary>
    /// 服务器地址
    /// </summary>
    [ObservableProperty]
    private string _apiDomain = GlobalConfig.ApiDomain.IsEmpty() ? "未设置" : GlobalConfig.ApiDomain;

    /// <summary>
    /// 网易云
    /// </summary>
    [ObservableProperty]
    private bool _isEnableNetEase = CheckEnablePlatform(PlatformEnum.NetEase);
    partial void OnIsEnableNetEaseChanged(bool value)
    {
        Task.Run(async () =>
        {
            await EnablePlatformAsync(PlatformEnum.NetEase, value);
        });
    }

    /// <summary>
    /// 酷我
    /// </summary>
    [ObservableProperty]
    private bool _isEnableKuWo = CheckEnablePlatform(PlatformEnum.KuWo);
    partial void OnIsEnableKuWoChanged(bool value)
    {
        Task.Run(async () =>
        {
            await EnablePlatformAsync(PlatformEnum.KuWo, value);
        });
    }

    /// <summary>
    /// 酷狗
    /// </summary>
    [ObservableProperty]
    private bool _isEnableKuGou = CheckEnablePlatform(PlatformEnum.KuGou);
    partial void OnIsEnableKuGouChanged(bool value)
    {
        Task.Run(async () =>
        {
            await EnablePlatformAsync(PlatformEnum.KuGou, value);
        });
    }

    /// <summary>
    /// 咪咕
    /// </summary>
    [ObservableProperty]
    private bool _isEnableMiGu = CheckEnablePlatform(PlatformEnum.MiGu);
    partial void OnIsEnableMiGuChanged(bool value)
    {
        Task.Run(async () =>
        {
            await EnablePlatformAsync(PlatformEnum.MiGu, value);
        });
    }

    /// <summary>
    /// 隐藏小于1分钟的歌曲
    /// </summary>
    [ObservableProperty]
    private bool _isHideShortMusic = GlobalConfig.MyUserSetting.Search.IsHideShortMusic;
    partial void OnIsHideShortMusicChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Search.IsHideShortMusic = value;
        WriteSearchConfigAsync();
    }

    /// <summary>
    /// 歌曲名或歌手名必须包含搜索词
    /// </summary>
    [ObservableProperty]
    private bool _isMatchSearchKey = GlobalConfig.MyUserSetting.Search.IsMatchSearchKey;
    partial void OnIsMatchSearchKeyChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Search.IsMatchSearchKey = value;
        WriteSearchConfigAsync();
    }

    /// <summary>
    /// 隐藏收费歌曲
    /// </summary>
    [ObservableProperty]
    private bool _isHideVipMusic = GlobalConfig.MyUserSetting.Search.IsHideVipMusic;
    partial void OnIsHideVipMusicChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Search.IsHideVipMusic = value;
        WriteSearchConfigAsync();
    }

    /// <summary>
    /// 仅WIFI下可播放
    /// </summary>
    [ObservableProperty]
    private bool _isWifiPlayOnly = GlobalConfig.MyUserSetting.Play.IsWifiPlayOnly;
    partial void OnIsWifiPlayOnlyChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Play.IsWifiPlayOnly = value;
        WritePlayConfigAsync();
    }

    /// <summary>
    /// 添加到歌单时自动播放
    /// </summary>
    [ObservableProperty]
    private bool _isPlayWhenAddToFavorite = GlobalConfig.MyUserSetting.Play.IsPlayWhenAddToFavorite;
    partial void OnIsPlayWhenAddToFavoriteChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Play.IsPlayWhenAddToFavorite = value;
        WritePlayConfigAsync();
    }

    /// <summary>
    /// 播放失败时自动跳到下一首
    /// </summary>
    [ObservableProperty]
    private bool _isAutoNextWhenFailed = GlobalConfig.MyUserSetting.Play.IsAutoNextWhenFailed;
    partial void OnIsAutoNextWhenFailedChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Play.IsAutoNextWhenFailed = value;
        WritePlayConfigAsync();
    }

    /// <summary>
    /// 播放我的歌单前清空播放列表
    /// </summary>
    [ObservableProperty]
    private bool _isCleanPlaylistWhenPlayMyFavorite = GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlayMyFavorite;
    partial void OnIsCleanPlaylistWhenPlayMyFavoriteChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlayMyFavorite = value;
        WritePlayConfigAsync();
    }

    /// <summary>
    /// 版本号
    /// </summary>
    [ObservableProperty]
    private string _versionString = GlobalConfig.CurrentVersionString;


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

    [RelayCommand]
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

    [RelayCommand]
    private async void GoToCacheClean()
    {
        await Shell.Current.GoToAsync($"{nameof(CacheCleanPage)}", true);
    }

    [RelayCommand]
    private async void GoToLog()
    {
        await Shell.Current.GoToAsync($"{nameof(LogPage)}", true);
    }

    [RelayCommand]
    private async void GoToLogin()
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}", true);
    }

    [RelayCommand]
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

    [RelayCommand]
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

    [RelayCommand]
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
