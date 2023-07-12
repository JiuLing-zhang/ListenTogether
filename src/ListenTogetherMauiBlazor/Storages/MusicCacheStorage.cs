using JiuLing.CommonLibs.ExtensionMethods;
using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor.Storages;
public class MusicCacheStorage : IMusicCacheStorage
{
    private readonly IKeyValueStorage _keyValueStorage;
    public MusicCacheStorage(IKeyValueStorage keyValueStorage)
    {
        _keyValueStorage = keyValueStorage;
    }
    public async Task<string> GetOrAddAsync(Playlist playlist, Func<Playlist, Task<MusicCacheMetadata?>> delegateFunc)
    {
        var fileName = await _keyValueStorage.GetAsync($"music-{playlist.Id}", "");
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
        var cachePath = Path.Combine(GlobalPath.MusicCacheDirectory, cacheFileNameOnly);
        await File.WriteAllBytesAsync(cachePath, musicCacheMetadata.Buffer);

        await _keyValueStorage.SetAsync($"music-{playlist.Id}", cachePath);
        return cachePath;
    }

    public Task CalcCacheSizeAsync(Action<double> delegage)
    {
        var files = Directory.GetFiles(GlobalPath.MusicCacheDirectory);
        foreach (var file in files)
        {
            var fi = new FileInfo(file);
            delegage(fi.Length);
        }
        return Task.CompletedTask;
    }

    public Task ClearCacheAsync(Action<double> delegage)
    {
        var files = Directory.GetFiles(GlobalPath.MusicCacheDirectory);
        foreach (var file in files)
        {
            try
            {
                var fi = new FileInfo(file);
                delegage(fi.Length);
                File.Delete(file);
            }
            catch (Exception ex)
            {
                Logger.Error("缓存文件删除失败。", ex);
            }
        }
        return Task.CompletedTask;
    }
}