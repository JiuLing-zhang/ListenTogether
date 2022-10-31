using ListenTogether.Services.MusicSwitchServer;
using NativeMediaMauiLib;

namespace ListenTogether.Services;
public class PlayerService
{
    private readonly HttpClient _httpClient;
    private readonly WifiOptionsService _wifiOptionsService;
    private readonly INativeAudioService _audioService;
    private readonly IMusicNetworkService _musicNetworkService;
    private readonly IMusicSwitchServerFactory _musicSwitchServerFactory;
    private readonly IMusicCacheService _musicCacheService;
    private System.Timers.Timer _timerPlayProgress;
    private bool _isBuffering = false;

    /// <summary>
    /// 是否正在播放
    /// </summary>
    public bool IsPlaying { get; set; }

    public Music CurrentMusic { get; set; } = null!;

    public MusicPosition CurrentPosition { get; set; } = new MusicPosition();

    public event EventHandler? NewMusicAdded;
    public event EventHandler? IsPlayingChanged;
    public event EventHandler? PositionChanged;

    public PlayerService(IMusicSwitchServerFactory musicSwitchServerFactory, INativeAudioService audioService, IMusicNetworkService musicNetworkService, IMusicServiceFactory musicServiceFactory, IMusicCacheService musicCacheService, WifiOptionsService wifiOptionsService, HttpClient httpClient)
    {
        _audioService = audioService;
        _audioService.PlayFinished += async (_, _) =>
        {
            var musicName = CurrentMusic?.Name;
            Logger.Info($"事件：播放完成->{musicName}");
            await Next();
        };
        _audioService.PlayFailed += async (_, _) =>
        {
            var musicName = CurrentMusic?.Name;
            Logger.Info($"事件：播放失败->{musicName}");
            await MediaFailed();
        };

        _audioService.Played += async (_, _) => await PlayAsync(CurrentMusic);
        _audioService.Paused += async (_, _) => await PlayAsync(CurrentMusic);
        _audioService.SkipToNext += async (_, _) => await Next();
        _audioService.SkipToPrevious += async (_, _) => await Previous();

        _httpClient = httpClient;
        _wifiOptionsService = wifiOptionsService;
        _musicNetworkService = musicNetworkService;
        _musicSwitchServerFactory = musicSwitchServerFactory;
        _musicCacheService = musicCacheService;

        _timerPlayProgress = new System.Timers.Timer();
        _timerPlayProgress.Interval = 1000;
        _timerPlayProgress.Elapsed += _timerPlayProgress_Elapsed;
        _timerPlayProgress.Start();
    }

    private void _timerPlayProgress_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
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
        Logger.Info($"内部调用：上一首");
        var previousMusic = await _musicSwitchServerFactory.Create(GlobalConfig.MyUserSetting.Player.PlayMode).GetPreviousAsync(CurrentMusic);
        Logger.Info($"内部调用（上一首）：准备播放->{previousMusic.Name}");
        CurrentMusic = null;
        await PlayAsync(previousMusic);
    }

    /// <summary>
    /// 下一首
    /// </summary>
    public async Task Next()
    {
        Logger.Info($"内部调用：下一首");
        var previousMusic = await _musicSwitchServerFactory.Create(GlobalConfig.MyUserSetting.Player.PlayMode).GetNextAsync(CurrentMusic);
        Logger.Info($"内部调用（下一首）：准备播放->{previousMusic.Name}");
        CurrentMusic = null;
        await PlayAsync(previousMusic);
    }

    private async Task MediaFailed()
    {
        if (GlobalConfig.MyUserSetting.Play.IsAutoNextWhenFailed)
        {
            Logger.Info($"内部调用：播放失败");
            await Next();
        }
    }

    public Task PlayAsync(Music music)
    {
        var isOtherMusic = CurrentMusic?.Id != music.Id;
        var isPlaying = isOtherMusic || !_audioService.IsPlaying;
        var position = isOtherMusic ? 0 : CurrentPosition.position.TotalMilliseconds;

        return PlayAsync(music, isPlaying, position);
    }

    public async Task PlayAsync(Music music, bool isPlaying, double position = 0)
    {
        Logger.Info($"内部调用：播放");
        if (music == null)
        {
            Logger.Info($"内部调用：歌曲不存在");
            return;
        }
        if (_isBuffering)
        {
            Logger.Info($"内部调用：缓冲中");
            return;
        }
        _isBuffering = true;

        var isOtherMusic = CurrentMusic?.Id != music.Id;
        if (isOtherMusic)
        {
            Logger.Info($"内部调用：暂停-->播放中");
            var musicPath = await GetMusicCachePathAsync(music);
            if (musicPath.IsEmpty())
            {
                Logger.Info($"内部调用：歌曲缓存文件不存在");
                _isBuffering = false;
                return;
            }
            CurrentMusic = music;

            if (_audioService.IsPlaying)
            {
                await InternalPauseAsync();
            }

            var image = await _httpClient.GetByteArrayAsync(music.ImageUrl);
            await _audioService.InitializeAsync(musicPath, new AudioMetadata(image, music.Name, music.Artist, music.Album));

            await InternalPlayPauseAsync(isPlaying, position);

            NewMusicAdded?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Logger.Info($"内部调用：播放中-->暂停");
            await InternalPlayPauseAsync(isPlaying, position);
        }
        _isBuffering = false;
        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
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

    private async Task<string> GetMusicCachePathAsync(Music music)
    {
        var cache = await _musicCacheService.GetOneByMuiscIdAsync(music.Id);
        if (cache != null && File.Exists(cache.FileName))
        {
            return cache.FileName;
        }

        if (!await _wifiOptionsService.HasWifiOrCanPlayWithOutWifiAsync())
        {
            Logger.Info($"内部调用：非WIFI");
            return "";
        }

        //重新获取播放链接
        MessagingCenter.Instance.Send<string, bool>("ListenTogether", "PlayerBuffering", true);
        var playUrl = await _musicNetworkService.GetPlayUrlAsync(music, GlobalConfig.MyUserSetting.Play.MusicFormatType);
        if (playUrl == null)
        {
            Logger.Info($"内部调用：未获取到播放地址");
            return "";
        }

        var cacheFileNameOnly = $"{music.Id}{GetPlayUrlFileExtension(playUrl)}";
        var cacheFileName = Path.Combine(GlobalConfig.MusicCacheDirectory, cacheFileNameOnly);
        var data = await _httpClient.GetByteArrayAsync(playUrl);
        File.WriteAllBytes(cacheFileName, data);
        string remark = $"{music.Artist}-{music.Name}";
        await _musicCacheService.AddOrUpdateAsync(music.Id, cacheFileName, remark);

        MessagingCenter.Instance.Send<string, bool>("ListenTogether", "PlayerBuffering", false);
        return cacheFileName;
    }

    private string GetPlayUrlFileExtension(string playUrl)
    {
        string pattern = """
            .+(?<Extension>\.\S+)\??\S*
            """;
        var (success, result) = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(playUrl, pattern);
        if (!success)
        {
            Logger.Info($"未能解析出后缀,{playUrl}");
            return "";
        }
        return result;
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