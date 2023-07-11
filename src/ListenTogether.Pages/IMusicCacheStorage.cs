using ListenTogether.Model;

namespace ListenTogether.Pages;
public interface IMusicCacheStorage
{
    Task<string> GetOrAddAsync(Playlist playlist, Func<Playlist, Task<MusicCacheMetadata?>> delegateFunc);

    Task CalcCacheSizeAsync(Action<double> delegage);
    Task ClearCacheAsync(Action<double> delegage);
}