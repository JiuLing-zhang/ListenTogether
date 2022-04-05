using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Maui.Services;
internal class PlayerServiceProxy : PlayerServiceAbstract
{
    PlayerServiceAbstract _playerService;
    private readonly IMusicNetworkService _musicNetworkService;
    private readonly IMusicService _musicService;
    private readonly IPlaylistService _playlistService;
    private readonly WifiOptionsService _wifiOptionsService;

    public override event EventHandler NewMusicAdded;
    public override event EventHandler IsPlayingChanged;
    public override event EventHandler PlayFinished;

    public override double CurrentPosition => _playerService.CurrentPosition;
    public override Music CurrentMusic => _playerService.CurrentMusic;
    public override bool IsPlaying => _playerService.IsPlaying;

    public PlayerServiceProxy(IAudioService audioService, IMusicNetworkService musicNetworkService, IMusicServiceFactory musicServiceFactory, IPlaylistServiceFactory playlistServiceFactory, WifiOptionsService wifiOptionsService)
    {
        _playerService = new PlayerService(audioService);
        _playerService.NewMusicAdded += NewMusicAdded;
        _playerService.IsPlayingChanged += IsPlayingChanged;
        _playerService.PlayFinished += PlayFinished;


        _musicNetworkService = musicNetworkService;
        _musicService = musicServiceFactory.Create();
        _playlistService = playlistServiceFactory.Create();
        _wifiOptionsService = wifiOptionsService;
    }

    public override async Task PlayAsync(Music music, double position = 0)
    {
        try
        {
            //网易的歌曲需要更新播放地址
            if (music.Platform == PlatformEnum.NetEase)
            {
                music = await _musicNetworkService.UpdatePlayUrl(music);
            }

            if (_wifiOptionsService.HasWifiOrCanPlayWithOutWifi())
            {
                await _playerService.PlayAsync(music, position);
            }
        }
        catch (Exception ex)
        {
            await MediaFailed();
        }
    }

    public override async Task PauseAsync()
    {
        await _playerService.PauseAsync();
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

            var music = await _musicService.GetOneAsync(playlist[nextId].MusicId);
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
            var music = await _musicService.GetOneAsync(randomMusicId);
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

            var music = await _musicService.GetOneAsync(playlist[nextId].MusicId);
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
            var music = await _musicService.GetOneAsync(randomMusicId);
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