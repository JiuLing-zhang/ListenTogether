using JiuLing.CommonLibs.Net;
using ListenTogether.Model.Enums;

namespace ListenTogether.Services;
public class PlayerService
{
    private IServiceProvider _services;
    private static IAudioService _audioService;
    private readonly IMusicNetworkService _musicNetworkService;
    private readonly WifiOptionsService _wifiOptionsService;

    private readonly static HttpClientHelper _httpClient = new HttpClientHelper();

    public double PositionMillisecond => _audioService.PositionMillisecond;
    public double DurationMillisecond => _audioService.DurationMillisecond;
    public bool IsMuted { set => _audioService.IsMuted = value; }
    public double Volume { set => _audioService.Volume = value; }

    /// <summary>
    /// 是否正在播放
    /// </summary>
    public bool IsPlaying { get; set; }

    /// <summary>
    /// 正在播放的歌曲信息
    /// </summary>
    public Music CurrentMusic => _currentMusic;
    private Music _currentMusic;

    public event EventHandler NewMusicAdded;
    public event EventHandler IsPlayingChanged;

    public PlayerService(IServiceProvider services, IAudioService audioService, IMusicNetworkService musicNetworkService, IMusicServiceFactory musicServiceFactory, IPlaylistServiceFactory playlistServiceFactory, WifiOptionsService wifiOptionsService)
    {
        _services = services;
        _audioService = audioService;
        _audioService.PlayFinished += async (_, _) => await Next();
        _audioService.PlayFailed += async (_, _) => await MediaFailed();

        _musicNetworkService = musicNetworkService;
        _wifiOptionsService = wifiOptionsService;
    }

    public async Task PauseAsync()
    {
        if (!IsPlaying)
        {
            return;
        }
        await _audioService.PauseAsync();
        IsPlaying = false;
        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task PlayAsync(Music music, double positionMillisecond = 0)
    {
        string musicPath = Path.Combine(GlobalConfig.MusicCacheDirectory, music.CacheFileName);
        if (!File.Exists(musicPath))
        {
            //缓存文件不存在时重新下载

            //网易的歌曲需要更新播放地址
            if (music.Platform == PlatformEnum.NetEase)
            {
                music = await _musicNetworkService.UpdatePlayUrl(music);
            }

            if (!await _wifiOptionsService.HasWifiOrCanPlayWithOutWifi())
            {
                await MediaFailed();
                return;
            }

            var data = await _httpClient.GetReadByteArray(music.PlayUrl);
            File.WriteAllBytes(musicPath, data);
        }

        var isChangeMusic = _currentMusic?.Id != music.Id;

        if (isChangeMusic)
        {
            _currentMusic = music;
            if (_audioService.IsPlaying)
            {
                await _audioService.PauseAsync();
            }
            await _audioService.InitializeAsync(musicPath);
            NewMusicAdded?.Invoke(this, EventArgs.Empty);
            await _audioService.PlayAsync(positionMillisecond);
        }
        else
        {
            await _audioService.PlayAsync(positionMillisecond);
        }
        IsPlaying = true;
        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
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
        var playlistService = _services.GetService<IPlaylistServiceFactory>().Create();

        var playlist = await playlistService.GetAllAsync();
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
        var playlistService = _services.GetService<IPlaylistServiceFactory>().Create();

        var playlist = await playlistService.GetAllAsync();
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
}