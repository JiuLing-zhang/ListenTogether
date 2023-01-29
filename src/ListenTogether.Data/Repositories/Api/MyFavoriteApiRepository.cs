using ListenTogether.Data.Interfaces;
using ListenTogether.Model;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Data.Repositories.Api;
public class MyFavoriteApiRepository : IMyFavoriteRepository
{
    public async Task<bool> NameExistAsync(string myFavoriteName)
    {
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.NameExist, myFavoriteName);
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(url);
        var obj = json.ToObject<Result>();
        if (obj == null || obj.Code != 0)
        {
            return true;
        }
        return false;
    }

    public async Task<MyFavorite?> AddOrUpdateAsync(MyFavorite myFavorite)
    {
        var requestMyFavorite = new MyFavoriteRequest()
        {
            Id = myFavorite.Id,
            Name = myFavorite.Name,
            ImageUrl = myFavorite.ImageUrl
        };
        string content = requestMyFavorite.ToJson();
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithToken.PostAsync(DataConfig.ApiSetting.MyFavorite.AddOrUpdate, sc);
        var json = await response.Content.ReadAsStringAsync();
        var obj = json.ToObject<Result<MyFavoriteResponse>>();
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

    public async Task<List<MyFavorite>> GetAllAsync()
    {
        var myFavoriteList = new List<MyFavorite>();
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(DataConfig.ApiSetting.MyFavorite.GetAll);
        var obj = json.ToObject<List<MyFavoriteResponse>>();

        if (obj == null)
        {
            return myFavoriteList;
        }

        myFavoriteList = obj.Select(x => new MyFavorite()
        {
            Id = x.Id,
            ImageUrl = x.ImageUrl,
            MusicCount = x.MusicCount,
            Name = x.Name,
            EditTime = x.EditTime
        }).ToList();
        return myFavoriteList;
    }

    public async Task<MyFavorite?> GetOneAsync(int id)
    {
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.Get, id);
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(url);
        var obj = json.ToObject<Result<MyFavoriteResponse>>();
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
        var obj = json.ToObject<Result>();
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> AddMusicToMyFavoriteAsync(int id, string musicId)
    {
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.AddMusic, id, musicId);
        var response = await DataConfig.HttpClientWithToken.PostAsync(url, null);
        var json = await response.Content.ReadAsStringAsync();

        var obj = json.ToObject<Result>();
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<List<MyFavoriteDetail>> GetMyFavoriteDetailAsync(int id)
    {
        var myMyFavoriteDetailList = new List<MyFavoriteDetail>();
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.GetDetail, id);
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(url);
        var obj = json.ToObject<List<MyFavoriteDetailResponse>>();

        if (obj == null)
        {
            return myMyFavoriteDetailList;
        }

        myMyFavoriteDetailList = obj.Select(x => new MyFavoriteDetail()
        {
            Id = x.Id,
            MyFavoriteId = x.MyFavoriteId,
            Music = x.Music
        }).ToList();
        return myMyFavoriteDetailList;
    }
    public async Task<bool> RemoveDetailAsync(int id)
    {
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.RemoveDetail, id);
        var response = await DataConfig.HttpClientWithToken.PostAsync(url, null);
        string json = await response.Content.ReadAsStringAsync();
        var obj = json.ToObject<Result>();
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<List<string>> GetAllMusicIdAsync()
    {
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(DataConfig.ApiSetting.MyFavorite.GetAllMusicId);
        var jsonResult = json.ToObject<List<string>>();
        if (jsonResult == null)
        {
            return new List<string>();
        }
        return jsonResult;
    }
}