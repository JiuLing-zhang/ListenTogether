using JiuLing.CommonLibs.ExtensionMethods;
using ListenTogether.Business.Interfaces;
using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Services.MusicSwitchServer;

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
    private readonly IMusicCacheService _musicCacheService;
    private readonly IMusicNetworkService _musicNetworkService;
    private readonly IPlaylistService _playlistService;

    public MusicPlayerService(IMusicSwitchServerFactory musicSwitchServerFactory, IPlayerService playerService, IMusicCacheService musicCacheService, IWifiOptionsService wifiOptionsService, IMusicNetworkService musicNetworkService, IHttpClientFactory httpClientFactory, IPlaylistService playlistService)
    {
        _musicSwitchServerFactory = musicSwitchServerFactory;
        _playerService = playerService;
        _musicCacheService = musicCacheService;
        _wifiOptionsService = wifiOptionsService;

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
        var cachePath = await GetMusicCachePathAsync(playlist.Id);
        if (cachePath.IsEmpty())
        {
            if (Settings.Environment.Play.IsWifiPlayOnly)
            {
                if (!await _wifiOptionsService.HasWifiOrCanPlayAsync())
                {
                    return;
                }
            }

            StartBuffering?.Invoke(this, EventArgs.Empty);

            //重新获取播放链接        
            var playUrl = await _musicNetworkService.GetPlayUrlAsync(playlist.Platform, playlist.IdOnPlatform, playlist.ExtendDataJson);
            if (playUrl.IsEmpty())
            {
                EndBuffer?.Invoke(this, EventArgs.Empty);
                Logger.Info($"播放地址获取失败。{playlist.IdOnPlatform}-{playlist.IdOnPlatform}-{playlist.Name}");
                await Next();
                return;
            }

            var cacheFileNameOnly = $"{playlist.Id}{GetPlayUrlFileExtension(playUrl)}";
            cachePath = Path.Combine(GlobalPath.MusicCacheDirectory, cacheFileNameOnly);
            var data = await _httpClientFactory.CreateClient().GetByteArrayAsync(playUrl);
            await File.WriteAllBytesAsync(cachePath, data);
            string remark = $"{playlist.Artist}-{playlist.Name}";
            await _musicCacheService.AddOrUpdateAsync(playlist.Id, cachePath, remark);

            EndBuffer?.Invoke(this, EventArgs.Empty);
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