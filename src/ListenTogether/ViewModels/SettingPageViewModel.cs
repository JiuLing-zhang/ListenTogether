using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Data.Api;
using ListenTogether.Model.Enums;

namespace ListenTogether.ViewModels;

public partial class SettingPageViewModel : ViewModelBase
{
    private readonly IEnvironmentConfigService _configService;
    private readonly IUserService _userService;
    private readonly IMusicNetworkService _musicNetworkService;
    private readonly ILoginDataStorage _loginDataStorage;
    public SettingPageViewModel(IEnvironmentConfigService configService, IUserService userService, IMusicNetworkService musicNetworkService, ILoginDataStorage loginDataStorage)
    {
        _configService = configService;
        _userService = userService;
        _musicNetworkService = musicNetworkService;
        _loginDataStorage = loginDataStorage;
    }
    public async Task InitializeAsync()
    {
        try
        {
            UserInfo = GetUserInfo();
        }
        catch (Exception ex)
        {
            await ToastService.Show("设置页加载失败");
            Logger.Error("设置页加载失败。", ex);
        }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    [ObservableProperty]
    private UserInfoViewModel? _userInfo;

    [ObservableProperty]
    private string _loginUsername = null!;

    [ObservableProperty]
    private string _loginPassword = null!;

    /// <summary>
    /// 页面标题
    /// </summary>
    public string Title => "设置";

    /// <summary>
    /// 自动检查更新
    /// </summary>
    [ObservableProperty]
    private bool _isAutoCheckUpdate = GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate;
    async partial void OnIsAutoCheckUpdateChanged(bool value)
    {
        GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate = value;
        await WriteGeneralConfigAsync();
    }

    /// <summary>
    /// 外观
    /// </summary>
    [ObservableProperty]
    private int _appThemeInt = GlobalConfig.MyUserSetting.General.AppThemeInt;
    async partial void OnAppThemeIntChanged(int value)
    {
        App.Current.UserAppTheme = (AppTheme)value;
        GlobalConfig.MyUserSetting.General.AppThemeInt = value;
        await WriteGeneralConfigAsync();
    }
    [RelayCommand]
    private async void ChooseAppThemeAsync()
    {
        string result = await Shell.Current.DisplayActionSheet("请选择外观：", null, null, "跟随系统", "浅色", "深色");
        switch (result)
        {
            case "跟随系统":
                AppThemeInt = 0;
                break;
            case "浅色":
                AppThemeInt = 1;
                break;
            case "深色":
                AppThemeInt = 2;
                break;
            default:
                break;
        }
    }

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
    async partial void OnIsHideShortMusicChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Search.IsHideShortMusic = value;
        await WriteSearchConfigAsync();
    }

    /// <summary>
    /// 歌曲名或歌手名必须包含搜索词
    /// </summary>
    [ObservableProperty]
    private bool _isMatchSearchKey = GlobalConfig.MyUserSetting.Search.IsMatchSearchKey;
    async partial void OnIsMatchSearchKeyChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Search.IsMatchSearchKey = value;
        await WriteSearchConfigAsync();
    }

    /// <summary>
    /// 仅WIFI下可播放
    /// </summary>
    [ObservableProperty]
    private bool _isWifiPlayOnly = GlobalConfig.MyUserSetting.Play.IsWifiPlayOnly;
    async partial void OnIsWifiPlayOnlyChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Play.IsWifiPlayOnly = value;
        await WritePlayConfigAsync();
    }

    /// <summary>
    /// 播放页面禁止屏幕关闭
    /// </summary>
    [ObservableProperty]
    private bool _isPlayingPageKeepScreenOn = GlobalConfig.MyUserSetting.Play.IsPlayingPageKeepScreenOn;
    async partial void OnIsPlayingPageKeepScreenOnChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Play.IsPlayingPageKeepScreenOn = value;
        await WritePlayConfigAsync();
    }

    /// <summary>
    /// 播放歌单前清空播放列表
    /// </summary>
    [ObservableProperty]
    private bool _isCleanPlaylistWhenPlaySongMenu = GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlaySongMenu;
    async partial void OnIsCleanPlaylistWhenPlaySongMenuChanged(bool value)
    {
        GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlaySongMenu = value;
        await WritePlayConfigAsync();
    }

    /// <summary>
    /// 音质设置
    /// </summary>
    [ObservableProperty]
    private string _musicFormatType = GlobalConfig.MyUserSetting.Play.MusicFormatType.ToString();
    async partial void OnMusicFormatTypeChanged(string value)
    {
        GlobalConfig.MyUserSetting.Play.MusicFormatType = (MusicFormatTypeEnum)Enum.Parse(typeof(MusicFormatTypeEnum), value);
        _musicNetworkService.SetMusicFormatType(GlobalConfig.MyUserSetting.Play.MusicFormatType);
        await WritePlayConfigAsync();
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

    private UserInfoViewModel? GetUserInfo()
    {
        if (_loginDataStorage.GetUsername().IsEmpty())
        {
            return null;
        }
        return new UserInfoViewModel()
        {
            Username = _loginDataStorage.GetUsername(),
            Nickname = _loginDataStorage.GetNickname(),
            Avatar = $"{GlobalConfig.ApiDomain}{_loginDataStorage.GetAvatar()}"
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
    private async void GoToCacheCleanAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(CacheCleanPage)}", true);
    }

    [RelayCommand]
    private async void GoToAutoCloseAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(AutoClosePage)}", true);
    }

    [RelayCommand]
    private async void GoToLogAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(LogPage)}", true);
    }

    [RelayCommand]
    private async void GoToLoginAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}", true);
    }

    [RelayCommand]
    private async void LogoutAsync()
    {
        var isOk = await Shell.Current.DisplayAlert("提示", "确定要退出吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        try
        {
            Loading("请稍候....");
            //服务端退出失败时不处理，直接本地清除登录信息
            await _userService.LogoutAsync();

            _loginDataStorage.Clear();
            UserInfo = null;
        }
        catch (Exception ex)
        {
            await ToastService.Show("网络出小差了");
            Logger.Error("退出失败。", ex);
        }
        finally
        {
            LoadComplete();
        }
    }

    [RelayCommand]
    private async void OpenUrlAsync(string url)
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
    }

    [RelayCommand]
    private async void CheckUpdateAsync()
    {
        try
        {
            Loading("正在检查更新....");
            await UpdateCheck.Do(false);
        }
        catch (Exception ex)
        {
            await ToastService.Show("检查失败，网络出小差了");
            Logger.Error("自动更新检查失败。", ex);
        }
        finally
        {
            LoadComplete();
        }
    }
}
