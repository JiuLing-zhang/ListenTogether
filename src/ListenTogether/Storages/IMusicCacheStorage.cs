namespace ListenTogether.Storages;
public interface IMusicCacheStorage
{
    Task<string> GetOrAddAsync(Playlist playlist, Func<Playlist, Task<MusicCacheMetadata>> delegateFunc);
    Task CalcCacheSizeAsync(Action<double> delegage);
    Task ClearCacheAsync();
}