using JiuLing.CommonLibs.Random;

namespace ListenTogether.Services.MusicSwitchServer;
public class MusicSwitchShuffleServer : IMusicSwitchServer
{
    private readonly IPlaylistService _playlistService;
    public MusicSwitchShuffleServer(IPlaylistService playlistService)
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

        Playlist randomPlaylist;
        do
        {
            randomPlaylist = RandomUtils.GetOneFromList<Playlist>(playlist);
        } while (randomPlaylist.MusicId == currentMusicId);
        return randomPlaylist;
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

        Playlist randomPlaylist;
        do
        {
            randomPlaylist = RandomUtils.GetOneFromList<Playlist>(playlist);
        } while (randomPlaylist.MusicId == currentMusicId);
        return randomPlaylist;
    }
}