using JiuLing.CommonLibs.ExtensionMethods;

using ListenTogether.Model;
using ListenTogether.Services.MusicSwitchServer;
using NetMusicLib;

namespace ListenTogether.Pages.Services;
public class MusicPlayerService
{
    public bool IsPlaying => _playerService.IsPlaying;
    public MusicMetadata? Metadata => _playerService.CurrentMetadata;
    public MusicPosition CurrentPosition => _playerService.CurrentPosition;

    public event EventHandler? NewMusicAdded;
    public event EventHandler? IsPlayingChanged;
    public event EventHandler? PositionChanged;

    public event EventHandler? StartBuffering;
    public event EventHandler? EndBuffer;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IWifiOptionsService _wifiOptionsService;
    private readonly IPlayerService _playerService;
    private readonly IMusicSwitchServerFactory _musicSwitchServerFactory;
    //TODO rename
    private readonly MusicNetPlatform _musicNetworkService;
    private readonly IPlaylistService _playlistService;
    private readonly IMusicCacheStorage _musicCacheStorage;
    private readonly ILogger<MusicPlayerService> _logger;
    public MusicPlayerService(IMusicSwitchServerFactory musicSwitchServerFactory, IPlayerService playerService, IWifiOptionsService wifiOptionsService, MusicNetPlatform musicNetworkService, IHttpClientFactory httpClientFactory, IPlaylistService playlistService, IMusicCacheStorage musicCacheStorage, ILogger<MusicPlayerService> logger)
    {
        _musicSwitchServerFactory = musicSwitchServerFactory;
        _playerService = playerService;
        _wifiOptionsService = wifiOptionsService;
        _logger = logger;

        _playerService.NewMusicAdded += (_, _) => NewMusicAdded?.Invoke(this, EventArgs.Empty);
        _playerService.IsPlayingChanged += (_, _) => IsPlayingChanged?.Invoke(this, EventArgs.Empty);
        _playerService.PositionChanged += (_, _) => PositionChanged?.Invoke(this, EventArgs.Empty);
        _playerService.PlayFinished += async (_, _) => await Next();
        _playerService.PlayFailed += async (_, _) => await MediaFailed();
        _playerService.PlayNext += async (_, _) => await Next();
        _playerService.PlayPrevious += async (_, _) => await Previous();
        _musicNetworkService = musicNetworkService;
        _httpClientFactory = httpClientFactory;
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
            return;
        }

        var cachePath = await _musicCacheStorage.GetOrAddAsync(playlist, async (x) =>
        {
            if (Settings.Environment.Play.IsWifiPlayOnly)
            {
                if (!await _wifiOptionsService.HasWifiOrCanPlayAsync())
                {
                    return null;
                }
            }

            StartBuffering?.Invoke(this, EventArgs.Empty);

            //重新获取播放链接        
            var playUrl = await _musicNetworkService.GetPlayUrlAsync((NetMusicLib.Enums.PlatformEnum)x.Platform, x.IdOnPlatform, x.ExtendDataJson);
            if (playUrl.IsEmpty())
            {
                EndBuffer?.Invoke(this, EventArgs.Empty);
                _logger.LogInformation($"播放地址获取失败。{x.IdOnPlatform}-{x.IdOnPlatform}-{x.Name}");
                return null;
            }

            var fileExtension = GetPlayUrlFileExtension(playUrl);
            var data = await _httpClientFactory.CreateClient().GetByteArrayAsync(playUrl);
            EndBuffer?.Invoke(this, EventArgs.Empty);

            return new MusicCacheMetadata(fileExtension, data);
        });

        if (cachePath.IsEmpty())
        {
            await Next();
        }

        var image = await _httpClientFactory.CreateClient().GetByteArrayAsync(playlist.ImageUrl);
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
            _logger.LogInformation($"未能解析出后缀,{playUrl}");
            return "";
        }
        return result;
    }

    /// <summary>
    /// 上一首
    /// </summary>
    public async Task Previous()
    {
        var previousMusic = await _musicSwitchServerFactory.Create(Settings.Environment.Player.PlayMode).GetPreviousAsync(_playerService.CurrentMetadata.Id);
        await PlayByPlaylistAsync(previousMusic);
    }

    /// <summary>
    /// 下一首
    /// </summary>
    public async Task Next()
    {
        var previousMusic = await _musicSwitchServerFactory.Create(Settings.Environment.Player.PlayMode).GetNextAsync(_playerService.CurrentMetadata.Id);
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