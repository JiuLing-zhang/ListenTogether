using System.Text.Json;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Response;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Data.Repositories.Api;
public class MusicApiRepository : IMusicRepository
{
    public async Task<Music?> GetOneAsync(string id)
    {
        string url = string.Format(GlobalConfig.ApiSetting.Music.Get, id);
        var json = await GlobalConfig.HttpClientWithToken.GetStringAsync(url);
        var obj = JsonSerializer.Deserialize<Result<MusicResponse>>(json);
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
            PlatformId = music.PlatformId,
            PlatformName = ((PlatformEnum)music.Platform).GetDescription(),
            Name = music.Name,
            Album = music.Album,
            Alias = music.Alias,
            Artist = music.Artist,
            CachePath = music.CachePath,
            Duration = music.Duration,
            ImageUrl = music.ImageUrl,
            Lyric = music.Lyric,
            PlayUrl = music.PlayUrl
        };
    }

    public async Task<bool> AddOrUpdateAsync(Music music)
    {
        string content = JsonSerializer.Serialize(music);
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await GlobalConfig.HttpClientWithToken.PostAsync(GlobalConfig.ApiSetting.Music.AddOrUpdate, sc);
        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> UpdateCacheAsync(string id, string cachePath)
    {
        var url = string.Format(GlobalConfig.ApiSetting.Music.UpdateCache, id, cachePath);
        var response = await GlobalConfig.HttpClientWithToken.PostAsync(url, null);
        var json = await response.Content.ReadAsStringAsync();

        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }
}