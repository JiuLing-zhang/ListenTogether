namespace ListenTogether.Services.MusicSwitchServer;

public class MusicSwitchRepeatOneServer : IMusicSwitchServer
{
    public Task<Music> GetPreviousAsync(Music currentMusic)
    {
        return Task.FromResult(currentMusic);
    }

    public Task<Music> GetNextAsync(Music currentMusic)
    {
        return Task.FromResult(currentMusic);
    }
}