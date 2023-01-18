using ListenTogether.Services.MusicSwitchServer;

namespace ListenTogether.Services;
public class MusicPlayerService
{
    public bool IsPlaying => _playerService.IsPlaying;
    public MusicMetadata Metadata => _playerService.CurrentMetadata;
    public MusicPosition CurrentPosition => _playerService.CurrentPosition;

    public event EventHandler? NewMusicAdded;
    public event EventHandler? IsPlayingChanged;
    public event EventHandler? PositionChanged;
    public event EventHandler? PlayFinished;
    public event EventHandler? PlayFailed;
    public event EventHandler? PlayNext;
    public event EventHandler? PlayPrevious;

    private readonly HttpClient _httpClient;
    private readonly WifiOptionsService _wifiOptionsService;
    private readonly PlayerService _playerService;
    private readonly IMusicSwitchServerFactory _musicSwitchServerFactory;
    private readonly IMusicCacheService _musicCacheService;
    private readonly IMusicNetworkService _musicNetworkService;
    private readonly IPlaylistService _playlistService;

    public MusicPlayerService(IMusicSwitchServerFactory musicSwitchServerFactory, PlayerService playerService, IMusicCacheService musicCacheService, WifiOptionsService wifiOptionsService, IMusicNetworkService musicNetworkService, HttpClient httpClient, IPlaylistService playlistService)
    {
        _musicSwitchServerFactory = musicSwitchServerFactory;
        _playerService = playerService;
        _musicCacheService = musicCacheService;
        _wifiOptionsService = wifiOptionsService;

        _playerService.NewMusicAdded += NewMusicAdded;
        _playerService.IsPlayingChanged += IsPlayingChanged;
        _playerService.PositionChanged += PositionChanged;
        _playerService.PlayFinished += PlayFinished;
        _playerService.PlayFailed += PlayFailed;
        _playerService.PlayNext += PlayNext;
        _playerService.PlayPrevious += PlayPrevious;
        _musicNetworkService = musicNetworkService;
        _httpClient = httpClient;
        _playlistService = playlistService;
    }

    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="music"></param>
    /// <returns></returns>
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
        var cachePath = await GetMusicCachePathAsync(playlist.MusicId);
        if (cachePath.IsEmpty())
        {
            if (!await _wifiOptionsService.HasWifiOrCanPlayWithOutWifiAsync())
            {
                return;
            }

            //重新获取播放链接
            MessagingCenter.Instance.Send<string, bool>("ListenTogether", "PlayerBuffering", true);
            var playUrl = await _musicNetworkService.GetPlayUrlAsync(playlist.Platform, playlist.MusicIdOnPlatform);
            if (playUrl.IsEmpty())
            {
                await ToastService.Show("播放地址获取失败");
                MessagingCenter.Instance.Send<string, bool>("ListenTogether", "PlayerBuffering", false);
                return;
            }

            var cacheFileNameOnly = $"{playlist.MusicId}{GetPlayUrlFileExtension(playUrl)}";
            cachePath = Path.Combine(GlobalConfig.MusicCacheDirectory, cacheFileNameOnly);
            var data = await _httpClient.GetByteArrayAsync(playUrl);
            await File.WriteAllBytesAsync(cachePath, data);
            string remark = $"{playlist.MusicArtist}-{playlist.MusicName}";
            await _musicCacheService.AddOrUpdateAsync(playlist.MusicId, cachePath, remark);

            MessagingCenter.Instance.Send<string, bool>("ListenTogether", "PlayerBuffering", false);
        }

        var image = await _httpClient.GetByteArrayAsync(playlist.MusicImageUrl);
        await _playerService.PlayAsync(
            new MusicMetadata(
                playlist.MusicId,
                playlist.MusicName,
                playlist.MusicArtist,
                playlist.MusicAlbum,
                image,
                cachePath)
            );
    }

    private async Task<string> GetMusicCachePathAsync(string musicId)
    {
        var cache = await _musicCacheService.GetOneByMuiscIdAsync(musicId);
        if (cache != null && File.Exists(cache.FileName))
        {
            return cache.FileName;
        }
        return "";
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
        if (GlobalConfig.MyUserSetting.Play.IsAutoNextWhenFailed)
        {
            Logger.Info($"内部调用：播放失败");
            await Next();
        }
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