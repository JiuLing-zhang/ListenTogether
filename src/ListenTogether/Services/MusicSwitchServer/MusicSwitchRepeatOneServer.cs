namespace ListenTogether.Services.MusicSwitchServer;

public class MusicSwitchRepeatOneServer : IMusicSwitchServer
{
    public async Task<Music> GetPreviousAsync(Music currentMusic)
    { 
        return currentMusic;
    }

    public async Task<Music> GetNextAsync(Music currentMusic)
    {
        return currentMusic;
    }
}