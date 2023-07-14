using ListenTogether.Data.Interface;
using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;
using ListenTogether.Model.Enums;

namespace ListenTogether.Data.Api.Repositories;
public class MusicService : IMusicService
{
    private readonly IHttpClientFactory _httpClientFactory = null!;
    public MusicService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<LocalMusic?> GetOneAsync(string id)
    {
        MusicResponse? music = null;
        try
        {
            string url = string.Format(DataConfig.ApiSetting.Music.Get, id);
            var json = await _httpClientFactory.CreateClient("WebAPI").GetStringAsync(url);
            var obj = json.ToObject<Result<MusicResponse>>();
            if (obj == null)
            {
                return default;
            }
            music = obj.Data as MusicResponse;
        }
        catch (Exception ex)
        {
            Logger.Error($"获取歌曲失败。{id}", ex);
        }

        if (music == null)
        {
            return default;
        }
        return new LocalMusic()
        {
            Id = id,
            Platform = (PlatformEnum)music.Platform,
            IdOnPlatform = music.IdOnPlatform,
            Name = music.Name,
            Album = music.Album,
            Artist = music.Artist,
            ImageUrl = music.ImageUrl,
            ExtendDataJson = music.ExtendDataJson,
        };
    }

    public async Task<bool> AddOrUpdateAsync(LocalMusic music)
    {
        var requestMusic = new MusicRequest()
        {
            Id = music.Id,
            Name = music.Name,
            Platform = music.Platform,
            IdOnPlatform = music.IdOnPlatform,
            Album = music.Album,
            Artist = music.Artist,
            ImageUrl = music.ImageUrl,
            ExtendDataJson = music.ExtendDataJson
        };
        string content = requestMusic.ToJson();
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpClientFactory.CreateClient("WebAPI").PostAsync(DataConfig.ApiSetting.Music.AddOrUpdate, sc);
            var json = await response.Content.ReadAsStringAsync();
            var obj = json.ToObject<Result>();
            if (obj == null || obj.Code != 0)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error($"更新歌曲信息失败。{music.ToJson()}", ex);
            return false;
        }
    }
}