using JiuLing.CommonLibs.Net;
using Microsoft.Extensions.Caching.Memory;

namespace ListenTogether.Utils;
internal class ImageCacheUtils
{
    private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private static readonly HttpClientHelper _httpClient = new HttpClientHelper();
    public static byte[] GetByteArrayUsingCache(string url)
    {
        return _cache.GetOrCreate(url, (x) =>
        {
            var task = Task.Run(Task<byte[]>? () =>
            {
                return _httpClient.GetReadByteArray(url);
            });
            return task.Result;
        });
    }
}