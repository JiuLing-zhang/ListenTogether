
using ListenTogether.Model;

namespace ListenTogether.Services.MusicSwitchServer;

public class MusicSwitchRepeatOneServer : IMusicSwitchServer
{
    private readonly IPlaylistService _playlistService;
    public MusicSwitchRepeatOneServer(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    public async Task<Playlist> GetPreviousAsync(string currentMusicId)
    {
        return await _playlistService.GetOneAsync(currentMusicId);
    }

    public async Task<Playlist> GetNextAsync(string currentMusicId)
    {
        return await _playlistService.GetOneAsync(currentMusicId);
    }
}