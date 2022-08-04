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
        if (playlist.Count == 0)
        {
            return currentMusic;
        }
        var musicService = _services.GetService<IMusicServiceFactory>().Create();
        if (playlist.Count == 1)
        {
            return await musicService.GetOneAsync(playlist[0].MusicId);
        }

        string randomMusicId;
        do
        {
            randomMusicId = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<Playlist>(playlist).MusicId;
        } while (randomMusicId == currentMusic.Id);
        return await musicService.GetOneAsync(randomMusicId);
    }
    public async Task<Music> GetNextAsync(Music currentMusic)
    {
        var playlist = await _playlistService.GetAllAsync();
        if (playlist.Count == 0)
        {
            return currentMusic;
        }
        var musicService = _services.GetService<IMusicServiceFactory>().Create();
        if (playlist.Count == 1)
        {
            return await musicService.GetOneAsync(playlist[0].MusicId);
        }

        string randomMusicId;
        do
        {
            randomMusicId = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<Playlist>(playlist).MusicId;
        } while (randomMusicId == currentMusic.Id);
        return await musicService.GetOneAsync(randomMusicId);
    }
}