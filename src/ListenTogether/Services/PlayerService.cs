using JiuLing.CommonLibs.Net;
using ListenTogether.Model.Enums;
using ListenTogether.Services.MusicSwitchServer;
using NativeMediaMauiLib;

namespace ListenTogether.Services;
public class PlayerService
{
    private readonly INativeAudioService _audioService;
    private readonly IMusicNetworkService _musicNetworkService;
    private readonly WifiOptionsService _wifiOptionsService;
    private readonly IMusicSwitchServerFactory _musicSwitchServerFactory;
    private System.Timers.Timer _timerPlayProgress;

    private readonly static HttpClientHelper _httpClient = new HttpClientHelper();

    /// <summary>
    /// 是否正在播放
    /// </summary>
    public bool IsPlaying { get; set; }

    public Music CurrentMusic { get; set; }

    public MusicPosition CurrentPosition { get; set; }

    public event EventHandler NewMusicAdded;
    public event EventHandler IsPlayingChanged;
    public event EventHandler PositionChanged;

    public PlayerService(IMusicSwitchServerFactory musicSwitchServerFactory, INativeAudioService audioService, IMusicNetworkService musicNetworkService, IMusicServiceFactory musicServiceFactory, WifiOptionsService wifiOptionsService)
    {
        _audioService = audioService;
        _audioService.PlayFinished += async (_, _) => await Next();
        _audioService.PlayFailed += async (_, _) => await MediaFailed();

        _musicNetworkService = musicNetworkService;
        _wifiOptionsService = wifiOptionsService;
        _musicSwitchServerFactory = musicSwitchServerFactory;

        _timerPlayProgress = new System.Timers.Timer();
        _timerPlayProgress.Interval = 1000;
        _timerPlayProgress.Elapsed += _timerPlayProgress_Elapsed;
        _timerPlayProgress.Start();
    }

    private void _timerPlayProgress_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (!IsPlaying)
        {
            return;
        }

        CurrentPosition = new MusicPosition()
        {
            position = TimeSpan.FromMilliseconds(_audioService.CurrentPosition),
            Duration = TimeSpan.FromMilliseconds(_audioService.CurrentDuration),
            PlayProgress = _audioService.CurrentPosition / _audioService.CurrentDuration
        };

        PositionChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 上一首
    /// </summary>
    public async Task Previous()
    {
        var previousMusic = await _musicSwitchServerFactory.Create(GlobalConfig.MyUserSetting.Player.PlayMode).GetPreviousAsync(CurrentMusic);
        await PlayAsync(previousMusic);
    }

    /// <summary>
    /// 下一首
    /// </summary>
    public async Task Next()
    {
        var previousMusic = await _musicSwitchServerFactory.Create(GlobalConfig.MyUserSetting.Player.PlayMode).GetNextAsync(CurrentMusic);
        await PlayAsync(previousMusic);
    }

    private async Task MediaFailed()
    {
        if (GlobalConfig.MyUserSetting.Play.IsAutoNextWhenFailed)
        {
            await Next();
        }
    }

    public async Task PlayAsync(Music music, double position = 0)
    {
        if (music == null)
        {
            return;
        }

        var isOtherMusic = CurrentMusic?.Id != music.Id;
        var isPlaying = isOtherMusic || !_audioService.IsPlaying;

        if (isOtherMusic)
        {
            await CacheMusicWhenNotExist(music);

            CurrentMusic = music;

            if (_audioService.IsPlaying)
            {
                await InternalPauseAsync();
            }

            await _audioService.InitializeAsync(CurrentMusic.PlayUrl.ToString());

            await InternalPlayPauseAsync(isPlaying, position);

            NewMusicAdded?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            await InternalPlayPauseAsync(isPlaying, position);
        }

        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
    }

    private async Task CacheMusicWhenNotExist(Music music)
    {
        string musicPath = Path.Combine(GlobalConfig.MusicCacheDirectory, music.CacheFileName);
        if (File.Exists(musicPath))
        {
            return;
        }

        //缓存文件不存在时重新下载
        //部分平台的播放链接会失效，重新获取
        if (music.Platform == PlatformEnum.NetEase || music.Platform == PlatformEnum.KuGou || music.Platform == PlatformEnum.KuWo)
        {
            music = await _musicNetworkService.UpdatePlayUrl(music);
        }

        if (!await _wifiOptionsService.HasWifiOrCanPlayWithOutWifiAsync())
        {
            await MediaFailed();
            return;
        }

        var data = await _httpClient.GetReadByteArray(music.PlayUrl);
        File.WriteAllBytes(musicPath, data);
    }

    private async Task InternalPlayPauseAsync(bool isPlaying, double position)
    {
        if (isPlaying)
        {
            await InternalPlayAsync(position);
        }
        else
        {
            await InternalPauseAsync();
        }
    }

    private async Task InternalPauseAsync()
    {
        await _audioService.PauseAsync();
        IsPlaying = false;
    }

    private async Task InternalPlayAsync(double position = 0)
    {
        var canPlay = await _wifiOptionsService.HasWifiOrCanPlayWithOutWifiAsync();

        if (!canPlay)
        {
            return;
        }

        await _audioService.PlayAsync(position);
        IsPlaying = true;
    }

    public async Task SetMuted(bool value)
    {
        await _audioService.SetMuted(value);
    }

    public async Task SetVolume(int value)
    {
        await _audioService.SetVolume(value);
    }
}