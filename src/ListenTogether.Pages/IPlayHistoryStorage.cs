namespace ListenTogether.Pages;
public interface IPlayHistoryStorage
{
    Task<string> GetLastMusicIdAsync();

    Task SetLastMusicIdAsync(string id);
}