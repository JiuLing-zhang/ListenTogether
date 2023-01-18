using ListenTogether.Data.Interfaces;
using ListenTogether.Model;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;
using ListenTogether.Model.Enums;

namespace ListenTogether.Data.Repositories.Api;
public class MusicApiRepository : IMusicRepository
{
    public async Task<Music?> GetOneAsync(string id)
    {
        string url = string.Format(DataConfig.ApiSetting.Music.Get, id);
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(url);
        var obj = json.ToObject<Result<MusicResponse>>();
        if (obj == null)
        {
            return default;
        }

        var music = obj.Data as MusicResponse;
        if (music == null)
        {
            return default;
        }
        return new Music()
        {
            Id = id,
            Platform = (PlatformEnum)music.Platform,
            PlatformInnerId = music.PlatformInnerId,
            //  PlatformName = ((PlatformEnum)music.Platform).GetDescription(),
            Name = music.Name,
            Album = music.Album,
            Artist = music.Artist,
            ImageUrl = music.ImageUrl,
            ExtendData = music.ExtendData,
        };
    }

    public async Task<bool> AddOrUpdateAsync(Music music)
    {
        var requestMusic = new MusicRequest()
        {
            Id = music.Id,
            Name = music.Name,
            Platform = (int)music.Platform,
            PlatformInnerId = music.PlatformInnerId,
            Album = music.Album,
            Artist = music.Artist,
            ImageUrl = music.ImageUrl,
            ExtendData = music.ExtendData
        };
        string content = requestMusic.ToJson();
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithToken.PostAsync(DataConfig.ApiSetting.Music.AddOrUpdate, sc);
        var json = await response.Content.ReadAsStringAsync();
        var obj = json.ToObject<Result>();
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> UpdateCacheAsync(string id, string cachePath)
    {
        var url = string.Format(DataConfig.ApiSetting.Music.UpdateCache, id, cachePath);
        var response = await DataConfig.HttpClientWithToken.PostAsync(url, null);
        var json = await response.Content.ReadAsStringAsync();

        var obj = json.ToObject<Result>();
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }
}