namespace ListenTogether.Services.MusicSwitchServer;
public interface IMusicSwitchServer
{
    public Task<Music> GetPreviousAsync(Music currentMusic);
    public Task<Music> GetNextAsync(Music currentMusic);
}