using ListenTogether.Services.MusicSwitchServer;
using ListenTogether.Storages;

namespace ListenTogether.Services;
public class MusicPlayerService
{
    public bool IsPlaying => _playerService.IsPlaying;
    public MusicMetadata? Metadata => _playerService.CurrentMetadata;
    public MusicPosition CurrentPosition => _playerService.CurrentPosition;

    public event EventHandler? NewMusicAdded;
    public event EventHandler? IsPlayingChanged;
    public event EventHandler? PositionChanged;

    private readonly HttpClient _httpClient;
    private readonly WifiOptionsService _wifiOptionsService;
    private readonly PlayerService _playerService;
    private readonly IMusicSwitchServerFactory _musicSwitchServerFactory;
    private readonly MusicNetPlatform _musicNetworkService;
    private readonly IPlaylistService _playlistService;
    private readonly IMusicCacheStorage _musicCacheStorage;
    public MusicPlayerService(IMusicSwitchServerFactory musicSwitchServerFactory, PlayerService playerService, WifiOptionsService wifiOptionsService, MusicNetPlatform musicNetworkService, HttpClient httpClient, IPlaylistService playlistService, IMusicCacheStorage musicCacheStorage)
    {
        _musicSwitchServerFactory = musicSwitchServerFactory;
        _playerService = playerService;
        _wifiOptionsService = wifiOptionsService;

        _playerService.NewMusicAdded += (_, _) => NewMusicAdded?.Invoke(this, EventArgs.Empty);
        _playerService.IsPlayingChanged += (_, _) => IsPlayingChanged?.Invoke(this, EventArgs.Empty);
        _playerService.PositionChanged += (_, _) => PositionChanged?.Invoke(this, EventArgs.Empty);
        _playerService.PlayFinished += async (_, _) => await Next();
        _playerService.PlayFailed += async (_, _) => await MediaFailed();
        _playerService.PlayNext += async (_, _) => await Next();
        _playerService.PlayPrevious += async (_, _) => await Previous();
        _musicNetworkService = musicNetworkService;
        _httpClient = httpClient;
        _playlistService = playlistService;
        _musicCacheStorage = musicCacheStorage;
    }

    /// <summary>
    /// 播放
    /// </summary>
    public async Task PlayAsync(string musicId)
    {
        var playlist = await _playlistService.GetOneAsync(musicId);
        await PlayByPlaylistAsync(playlist);
    }

    private async Task PlayByPlaylistAsync(Playlist playlist)
    {
        if (playlist == null)
        {
            await ToastService.Show("播放列表加载失败");
            return;
        }

        var cachePath = await _musicCacheStorage.GetOrAddAsync(playlist, async (x) =>
        {
            if (!await _wifiOptionsService.HasWifiOrCanPlayWithOutWifiAsync())
            {
                return default;
            }

            string key = GuidUtils.GetFormatN();
            LoadingService.Loading(key, "歌曲缓冲中....");

            ///重新获取播放链接        
            var playUrl = await _musicNetworkService.GetPlayUrlAsync(x.Platform, x.IdOnPlatform, x.ExtendDataJson);
            if (playUrl.IsEmpty())
            {
                LoadingService.LoadComplete(key);
                Logger.Info($"播放地址获取失败。{x.IdOnPlatform}-{x.IdOnPlatform}-{x.Name}");
                return default;
            }

            var fileExtension = GetPlayUrlFileExtension(playUrl);
            var data = await _httpClient.GetByteArrayAsync(playUrl);
            LoadingService.LoadComplete(key);
            return new MusicCacheMetadata(fileExtension, data);
        });

        if (cachePath.IsEmpty())
        {
            await Next();
        }

        var image = await _httpClient.GetByteArrayAsync(playlist.ImageUrl);
        await _playerService.PlayAsync(
            new MusicMetadata(
                playlist.Id,
                playlist.Name,
                playlist.Artist,
                playlist.Album,
                image,
                cachePath)
            );
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

    /// <summary>
    /// 上一首
    /// </summary>
    public async Task Previous()
    {
        var previousMusic = await _musicSwitchServerFactory.Create(GlobalConfig.MyUserSetting.Player.PlayMode).GetPreviousAsync(_playerService.CurrentMetadata.Id);
        await PlayByPlaylistAsync(previousMusic);
    }

    /// <summary>
    /// 下一首
    /// </summary>
    public async Task Next()
    {
        var previousMusic = await _musicSwitchServerFactory.Create(GlobalConfig.MyUserSetting.Player.PlayMode).GetNextAsync(_playerService.CurrentMetadata.Id);
        await PlayByPlaylistAsync(previousMusic);
    }

    private async Task MediaFailed()
    {
        await Next();
    }

    public async Task SetPlayPosition(double positionMillisecond)
    {
        await _playerService.SetPlayPosition(positionMillisecond);
    }

    public async Task SetMuted(bool value)
    {
        await _playerService.SetMuted(value);
    }

    public async Task SetVolume(int value)
    {
        await _playerService.SetVolume(value);
    }
}