namespace ListenTogether.Services.MusicSwitchServer;
public class MusicSwitchRepeatListServer : IMusicSwitchServer
{
    private IServiceProvider _services;
    private readonly IPlaylistService _playlistService;
    public MusicSwitchRepeatListServer(IServiceProvider services, IPlaylistService playlistService)
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

        int nextId = 0;
        for (int i = 0; i < playlist.Count; i++)
        {
            if (playlist[i].MusicId == currentMusic.Id)
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
        return await musicService.GetOneAsync(playlist[nextId].MusicId);
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

        int nextId = 0;
        for (int i = 0; i < playlist.Count; i++)
        {
            if (playlist[i].MusicId == currentMusic.Id)
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

        return await musicService.GetOneAsync(playlist[nextId].MusicId);
    }
}