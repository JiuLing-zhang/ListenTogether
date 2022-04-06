using System.Text.Json;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Response;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Data.Repositories.Api;
public class MyFavoriteApiRepository : IMyFavoriteRepository
{
    /// <summary>
    /// 按照名称添加或更新
    /// </summary>
    /// <returns>添加或更新后的id</returns>
    public async Task<MyFavorite?> AddOrUpdateByNameAsync(MyFavorite myFavorite)
    {
        string content = JsonSerializer.Serialize(myFavorite);
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithToken.PostAsync(DataConfig.ApiSetting.MyFavorite.AddOrUpdate, sc);
        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<Result<MyFavoriteResponse>>(json);
        if (obj == null || obj.Code != 0 || obj.Data == null)
        {
            return default;
        }

        return new MyFavorite()
        {
            Id = obj.Data.Id,
            Name = obj.Data.Name,
            ImageUrl = obj.Data.ImageUrl,
            MusicCount = obj.Data.MusicCount
        };
    }

    public async Task<List<MyFavorite>?> GetAllAsync()
    {
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(DataConfig.ApiSetting.MyFavorite.GetAll);
        var obj = JsonSerializer.Deserialize<List<MyFavoriteResponse>>(json);

        if (obj == null)
        {
            return default;
        }

        return obj.Select(x => new MyFavorite()
        {
            Id = x.Id,
            ImageUrl = x.ImageUrl,
            MusicCount = x.MusicCount,
            Name = x.Name,
        }).ToList();
    }

    public async Task<MyFavorite?> GetOneAsync(int id)
    {
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.Get, id);
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(url);
        var obj = JsonSerializer.Deserialize<Result<MyFavoriteResponse>>(json);
        if (obj == null)
        {
            return default;
        }

        var myFavorite = obj.Data as MyFavoriteResponse;
        if (myFavorite == null)
        {
            return default;
        }
        return new MyFavorite()
        {
            Id = myFavorite.Id,
            Name = myFavorite.Name,
            ImageUrl = myFavorite.ImageUrl,
            MusicCount = myFavorite.MusicCount
        };
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.Remove, id);
        var response = await DataConfig.HttpClientWithToken.PostAsync(url, null);
        string json = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> AddMusicToMyFavorite(int id, Music music)
    {
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.AddMusic, id);

        string content = JsonSerializer.Serialize(music);
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithToken.PostAsync(url, sc);
        var json = await response.Content.ReadAsStringAsync();

        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<List<MyFavoriteDetail>?> GetMyFavoriteDetail(int id)
    {
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.GetDetail, id);
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(url);
        var obj = JsonSerializer.Deserialize<List<MyFavoriteDetailResponse>>(json);

        if (obj == null)
        {
            return default;
        }

        return obj.Select(x => new MyFavoriteDetail()
        {
            Id = x.Id,
            MusicId = x.MusicId,
            Platform = (PlatformEnum)x.Platform,
            MusicName = x.MusicName,
            MyFavoriteId = x.MyFavoriteId,
            MusicAlbum = x.MusicAlbum,
            MusicArtist = x.MusicArtist,
        }).ToList();
    }
}