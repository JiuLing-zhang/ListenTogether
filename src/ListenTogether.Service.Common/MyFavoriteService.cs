using ListenTogether.Data.Interface;
using ListenTogether.Model;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Data.Api.Repositories;
public class MyFavoriteService : IMyFavoriteService
{
    private readonly ILogger<MyFavoriteService> _logger;
    private readonly IHttpClientFactory _httpClientFactory = null!;
    public MyFavoriteService(IHttpClientFactory httpClientFactory, ILogger<MyFavoriteService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    public async Task<bool> NameExistAsync(string myFavoriteName)
    {
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.NameExist, myFavoriteName);
        try
        {
            var json = await _httpClientFactory.CreateClient("WebAPI").GetStringAsync(url);
            var obj = json.ToObject<Result>();
            if (obj == null || obj.Code != 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"歌单名称校验失败。名称：{myFavoriteName}");
            return true;
        }
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
        MyFavoriteResponse? myFavoriteResponse = null;
        try
        {
            var response = await _httpClientFactory.CreateClient("WebAPI").PostAsync(DataConfig.ApiSetting.MyFavorite.AddOrUpdate, sc);
            var json = await response.Content.ReadAsStringAsync();
            var obj = json.ToObject<Result<MyFavoriteResponse>>();
            if (obj != null && obj.Code == 0)
            {
                myFavoriteResponse = obj.Data;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"歌单更新失败。名称：{myFavorite.ToJson()}");
        }

        if (myFavoriteResponse == null)
        {
            return default;
        }

        return new MyFavorite()
        {
            Id = myFavoriteResponse.Id,
            Name = myFavoriteResponse.Name,
            ImageUrl = myFavoriteResponse.ImageUrl,
            MusicCount = myFavoriteResponse.MusicCount
        };
    }

    public async Task<List<MyFavorite>> GetAllAsync()
    {
        var myFavoriteList = new List<MyFavorite>();
        List<MyFavoriteResponse>? obj = null;
        try
        {
            var json = await _httpClientFactory.CreateClient("WebAPI").GetStringAsync(DataConfig.ApiSetting.MyFavorite.GetAll);
            obj = json.ToObject<List<MyFavoriteResponse>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"获取歌单列表失败。");
        }
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
        MyFavoriteResponse? myFavorite = null;
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.Get, id);
        try
        {
            var json = await _httpClientFactory.CreateClient("WebAPI").GetStringAsync(url);
            var obj = json.ToObject<Result<MyFavoriteResponse>>();
            if (obj != null)
            {
                myFavorite = obj.Data;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"获取歌单失败。{id}");
        }
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
        try
        {
            var url = string.Format(DataConfig.ApiSetting.MyFavorite.Remove, id);
            var response = await _httpClientFactory.CreateClient("WebAPI").PostAsync(url, null);
            string json = await response.Content.ReadAsStringAsync();
            var obj = json.ToObject<Result>();
            if (obj == null || obj.Code != 0)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"删除歌单失败。{id}");
            return false;
        }
    }

    public async Task<bool> AddMusicToMyFavoriteAsync(int id, string musicId)
    {
        try
        {
            var url = string.Format(DataConfig.ApiSetting.MyFavorite.AddMusic, id, musicId);
            var response = await _httpClientFactory.CreateClient("WebAPI").PostAsync(url, null);
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
            _logger.LogError(ex, $"添加歌曲到歌单失败。歌单{id}，歌曲{musicId}");
            return false;
        }
    }

    public async Task<List<MyFavoriteDetail>> GetMyFavoriteDetailAsync(int id)
    {
        var myMyFavoriteDetailList = new List<MyFavoriteDetail>();
        var url = string.Format(DataConfig.ApiSetting.MyFavorite.GetDetail, id);
        List<MyFavoriteDetailResponse>? obj = null;
        try
        {
            var json = await _httpClientFactory.CreateClient("WebAPI").GetStringAsync(url);
            obj = json.ToObject<List<MyFavoriteDetailResponse>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"获取歌单详情失败。{id}");
        }

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
        try
        {
            var url = string.Format(DataConfig.ApiSetting.MyFavorite.RemoveDetail, id);
            var response = await _httpClientFactory.CreateClient("WebAPI").PostAsync(url, null);
            string json = await response.Content.ReadAsStringAsync();
            var obj = json.ToObject<Result>();
            if (obj == null || obj.Code != 0)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"删除歌单详情失败。{id}");
            return false;
        }
    }
}