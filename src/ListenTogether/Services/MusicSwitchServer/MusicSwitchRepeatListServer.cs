namespace ListenTogether.Services.MusicSwitchServer;
public class MusicSwitchRepeatListServer : IMusicSwitchServer
{
    private readonly IPlaylistService _playlistService;
    public MusicSwitchRepeatListServer(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    public async Task<Playlist?> GetPreviousAsync(string currentMusicId)
    {
        var playlist = await _playlistService.GetAllAsync();
        if (playlist == null || playlist.Count == 0)
        {
            return default;
        }

        if (playlist.Count == 1)
        {
            return playlist[0];
        }

        int nextId = 0;
        for (int i = 0; i < playlist.Count; i++)
        {
            if (playlist[i].Id == currentMusicId)
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
        return playlist[nextId];
    }
    public async Task<Playlist?> GetNextAsync(string currentMusicId)
    {
        var playlist = await _playlistService.GetAllAsync();
        if (playlist == null || playlist.Count == 0)
        {
            return default;
        }

        if (playlist.Count == 1)
        {
            return playlist[0];
        }

        int nextId = 0;
        for (int i = 0; i < playlist.Count; i++)
        {
            if (playlist[i].Id == currentMusicId)
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
        return playlist[nextId];
    }
}