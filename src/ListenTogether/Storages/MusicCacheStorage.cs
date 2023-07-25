using Microsoft.Extensions.Logging;

namespace ListenTogether.Storages;
public class MusicCacheStorage : IMusicCacheStorage
{
    private readonly ILogger<MusicCacheStorage> _logger;
    public MusicCacheStorage(ILogger<MusicCacheStorage> logger)
    {
        _logger = logger;
    }
    public Task CalcCacheSizeAsync(Action<double> delegage)
    {
        var files = Directory.GetFiles(GlobalConfig.MusicCacheDirectory);
        foreach (var file in files)
        {
            var fi = new FileInfo(file);
            delegage(fi.Length);
        }
        return Task.CompletedTask;
    }

    public Task ClearCacheAsync()
    {
        var files = Directory.GetFiles(GlobalConfig.MusicCacheDirectory);
        foreach (var file in files)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "缓存文件删除失败。");
            }
        }
        return Task.CompletedTask;
    }

    public async Task<string> GetOrAddAsync(Playlist playlist, Func<Playlist, Task<MusicCacheMetadata>> delegateFunc)
    {
        var fileName = Preferences.Get($"music-{playlist.Id}", "");
        if (fileName.IsNotEmpty() && File.Exists(fileName))
        {
            return fileName;
        }
        var musicCacheMetadata = await delegateFunc(playlist);
        if (musicCacheMetadata == null)
        {
            return default;
        }

        var cacheFileNameOnly = $"{playlist.Id}{musicCacheMetadata.FileExtension}";
        var cachePath = Path.Combine(GlobalConfig.MusicCacheDirectory, cacheFileNameOnly);
        await File.WriteAllBytesAsync(cachePath, musicCacheMetadata.Buffer);

        Preferences.Set($"music-{playlist.Id}", cachePath);
        return cachePath;
    }
}