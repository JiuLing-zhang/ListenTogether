namespace ListenTogether.Services.MusicSwitchServer;
public class MusicSwitchShuffleServer : IMusicSwitchServer
{
    private IServiceProvider _services;
    private readonly IPlaylistService _playlistService;
    public MusicSwitchShuffleServer(IServiceProvider services, IPlaylistService playlistService)
    {
        _services = services;
        _playlistService = playlistService;
    }
    public async Task<Music> GetPreviousAsync(Music currentMusic)
    {
        var playlist = await _playlistService.GetAllAsync();
        if (playlist == null || playlist.Count == 0)
        {
            return currentMusic;
        }
        var musicService = _services.GetRequiredService<IMusicServiceFactory>().Create();
        if (playlist.Count == 1)
        {
            return await musicService.GetOneAsync(playlist[0].MusicId) ?? throw new Exception("歌曲信息读取失败");
        }

        string randomMusicId;
        do
        {
            randomMusicId = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<Playlist>(playlist).MusicId;
        } while (randomMusicId == currentMusic.Id);
        return await musicService.GetOneAsync(randomMusicId) ?? throw new Exception("歌曲信息读取失败");
    }
    public async Task<Music> GetNextAsync(Music currentMusic)
    {
        var playlist = await _playlistService.GetAllAsync();
        if (playlist == null || playlist.Count == 0)
        {
            return currentMusic;
        }
        var musicService = _services.GetRequiredService<IMusicServiceFactory>().Create();
        if (playlist.Count == 1)
        {
            return await musicService.GetOneAsync(playlist[0].MusicId) ?? throw new Exception("歌曲信息读取失败");
        }

        string randomMusicId;
        do
        {
            randomMusicId = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<Playlist>(playlist).MusicId;
        } while (randomMusicId == currentMusic.Id);
        return await musicService.GetOneAsync(randomMusicId) ?? throw new Exception("歌曲信息读取失败");
    }
}