using JiuLing.CommonLibs.Net;
using ListenTogether.Model;
using ListenTogether.Model.Enums;
using NativeMediaMauiLib;

namespace ListenTogether.Services;
public class PlayerService
{
    private IServiceProvider _services;
    private readonly INativeAudioService _audioService;
    private readonly IMusicNetworkService _musicNetworkService;
    private readonly IPlaylistService _playlistService;
    private readonly WifiOptionsService _wifiOptionsService;
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

    public PlayerService(IServiceProvider services, INativeAudioService audioService, IMusicNetworkService musicNetworkService, IMusicServiceFactory musicServiceFactory, IPlaylistService playlistService, WifiOptionsService wifiOptionsService)
    {
        _services = services;

        _audioService = audioService;
        _audioService.PlayFinished += async (_, _) => await Next();
        _audioService.PlayFailed += async (_, _) => await MediaFailed();

        _playlistService = playlistService;
        _musicNetworkService = musicNetworkService;
        _wifiOptionsService = wifiOptionsService;

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
        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatOne)
        {
            await PlayAsync(CurrentMusic);
            return;
        }
        var musicService = _services.GetService<IMusicServiceFactory>().Create();

        var playlist = await _playlistService.GetAllAsync();
        if (playlist.Count == 0)
        {
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatList)
        {
            int nextId = 0;
            for (int i = 0; i < playlist.Count; i++)
            {
                if (playlist[i].MusicId == CurrentMusic.Id)
                {
                    nextId = i - 1;
                    break;
                }
            }
            //列表第一首
            if (nextId < 0)
            {
                nextId = playlist.Count - 1;
            }

            var music = await musicService.GetOneAsync(playlist[nextId].MusicId);
            await PlayAsync(music);
            return;
        }
        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.Shuffle)
        {
            if (playlist.Count <= 1)
            {
                await PlayAsync(CurrentMusic);
                return;
            }

            string randomMusicId;
            do
            {
                randomMusicId = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<Playlist>(playlist).MusicId;
            } while (randomMusicId == CurrentMusic.Id);
            var music = await musicService.GetOneAsync(randomMusicId);
            await PlayAsync(music);
        }
    }

    /// <summary>
    /// 下一首
    /// </summary>
    public async Task Next()
    {
        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatOne)
        {
            await PlayAsync(CurrentMusic);
            return;
        }

        var musicService = _services.GetService<IMusicServiceFactory>().Create();

        var playlist = await _playlistService.GetAllAsync();
        if (playlist.Count == 0)
        {
            return;
        }
        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatList)
        {
            int nextId = 0;
            for (int i = 0; i < playlist.Count; i++)
            {
                if (playlist[i].MusicId == CurrentMusic.Id)
                {
                    nextId = i + 1;
                    break;
                }
            }
            //列表最后一首
            if (playlist.Count == nextId)
            {
                nextId = 0;
            }

            var music = await musicService.GetOneAsync(playlist[nextId].MusicId);
            await PlayAsync(music);
            return;
        }
        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.Shuffle)
        {
            if (playlist.Count <= 1)
            {
                await PlayAsync(CurrentMusic);
                return;
            }

            string randomMusicId;
            do
            {
                randomMusicId = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<Playlist>(playlist).MusicId;
            } while (randomMusicId == CurrentMusic.Id);
            var music = await musicService.GetOneAsync(randomMusicId);
            await PlayAsync(music);
        }
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