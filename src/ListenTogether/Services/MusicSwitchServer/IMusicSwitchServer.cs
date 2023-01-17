namespace ListenTogether.Services.MusicSwitchServer;
public interface IMusicSwitchServer
{
    public Task<Playlist?> GetPreviousAsync(string currentMusicId);
    public Task<Playlist?> GetNextAsync(string currentMusicId);
}