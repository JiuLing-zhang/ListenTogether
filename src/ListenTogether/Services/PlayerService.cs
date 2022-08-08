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

    public MusicPosition CurrentPosition { get; set; } = new MusicPosition();

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

        CurrentPosition.position = TimeSpan.FromMilliseconds(_audioService.CurrentPositionMillisecond);
        CurrentPosition.Duration = TimeSpan.FromMilliseconds(_audioService.CurrentDurationMillisecond);
        CurrentPosition.PlayProgress = _audioService.CurrentPositionMillisecond / _audioService.CurrentDurationMillisecond;

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

    public async Task PlayAsync(Music music)
    {
        if (music == null)
        {
            return;
        }

        bool isOtherMusic;
        if (CurrentMusic == null || CurrentMusic.Id != music.Id)
        {
            isOtherMusic = true;
        }
        else
        {
            isOtherMusic = false;
        }
        var isPlaying = isOtherMusic || !_audioService.IsPlaying;
        var positionMilliseconds = isOtherMusic ? 0 : CurrentPosition.position.TotalMilliseconds;

        if (isOtherMusic)
        {
            var musicPath = await GetMusicCachePath(music);
            if (musicPath.IsEmpty())
            {
                return;
            }
            CurrentMusic = music;

            if (_audioService.IsPlaying)
            {
                await InternalPauseAsync();
            }

            await _audioService.InitializeAsync(musicPath);

            await InternalPlayPauseAsync(isPlaying, positionMilliseconds);

            NewMusicAdded?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            await InternalPlayPauseAsync(isPlaying, positionMilliseconds);
        }

        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
    }

    private async Task<string> GetMusicCachePath(Music music)
    {
        string musicPath = Path.Combine(GlobalConfig.MusicCacheDirectory, music.CacheFileName);
        if (File.Exists(musicPath))
        {
            return musicPath;
        }

        if (!await _wifiOptionsService.HasWifiOrCanPlayWithOutWifiAsync())
        {
            //TODO 非WIFI，这里加个之类的
            return "";
        }
        //缓存文件不存在时重新下载
        //部分平台的播放链接会失效，重新获取
        if (music.Platform == PlatformEnum.NetEase || music.Platform == PlatformEnum.KuGou || music.Platform == PlatformEnum.KuWo)
        {
            music = await _musicNetworkService.UpdatePlayUrl(music);
        }

        var data = await _httpClient.GetReadByteArray(music.PlayUrl);
        File.WriteAllBytes(musicPath, data);
        return musicPath;
    }

    private async Task InternalPlayPauseAsync(bool isPlaying, double positionMilliseconds)
    {
        if (isPlaying)
        {
            await InternalPlayAsync(positionMilliseconds);
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

    private async Task InternalPlayAsync(double positionMillisecond = 0)
    {
        await _audioService.PlayAsync(positionMillisecond);
        IsPlaying = true;
    }
    public async Task SetPlayPosition(double positionMillisecond)
    {
        await InternalPlayAsync(positionMillisecond);
        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
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