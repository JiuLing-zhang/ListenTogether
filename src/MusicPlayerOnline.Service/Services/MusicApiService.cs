using System.Text.Json;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service.Services;
internal class MusicApiService : IMusicService
{
    public async Task<Result<MusicDto>> GetOneAsync(string id)
    {
        string url = string.Format(GlobalConfig.ApiSetting.Music.Get, id);
        var json = await HttpClientSingleton.Instance().GetStringAsync(url);
        var obj = JsonSerializer.Deserialize<Result<MusicDto>>(json);
        if (obj == null)
        {
            return new Result<MusicDto>(999, "连接服务器失败", null);
        }
        return obj;
    }

    public async Task<Result> AddOrUpdateAsync(Music music)
    {
        var json = await HttpClientSingleton.Instance().PostReadAsStringAsync(GlobalConfig.ApiSetting.Music.AddOrUpdate, music);
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null)
        {
            return new Result(999, "连接服务器失败");
        }
        return obj;
    }

    public async Task<Result> UpdateCacheAsync(string id, string cachePath)
    {
        var url = string.Format(GlobalConfig.ApiSetting.Music.UpdateCache, id, cachePath);
        var json = await HttpClientSingleton.Instance().PostStringAsync(url);
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null)
        {
            return new Result(999, "连接服务器失败");
        }
        return obj;
    }
}