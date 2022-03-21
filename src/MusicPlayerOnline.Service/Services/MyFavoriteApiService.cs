using System.Text.Json;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service.Services;
internal class MyFavoriteApiService : IMyFavoriteService
{
    public async Task<Result> AddMusicToMyFavorite(int id, MyFavoriteDetail music)
    {
        var url = string.Format(GlobalConfig.ApiSetting.MyFavorite.AddMusic, id);
        var json = await HttpClientSingleton.Instance().PostReadAsStringAsync(url, music);
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null)
        {
            return new Result(999, "连接服务器失败");
        }
        return obj;
    }

    public async Task<Result> AddOrUpdateAsync(MyFavorite myFavorite)
    {
        var json = await HttpClientSingleton.Instance().PostReadAsStringAsync(GlobalConfig.ApiSetting.MyFavorite.AddOrUpdate, myFavorite);
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null)
        {
            return new Result(999, "连接服务器失败");
        }
        return obj;
    }

    public async Task<List<MyFavoriteDto>?> GetAllAsync()
    {
        var json = await HttpClientSingleton.Instance().GetStringAsync(GlobalConfig.ApiSetting.MyFavorite.GetAll);
        var obj = JsonSerializer.Deserialize<List<MyFavoriteDto>>(json);
        return obj ?? default;
    }

    public async Task<List<MyFavoriteDetailDto>?> GetMyFavoriteDetail(int id)
    {
        var url = string.Format(GlobalConfig.ApiSetting.MyFavorite.GetDetail, id);
        var json = await HttpClientSingleton.Instance().GetStringAsync(url);
        var obj = JsonSerializer.Deserialize<List<MyFavoriteDetailDto>>(json);
        return obj ?? default;
    }

    public async Task<Result<MyFavoriteDto>> GetOneAsync(int id)
    {
        var url = string.Format(GlobalConfig.ApiSetting.MyFavorite.Get, id);
        var json = await HttpClientSingleton.Instance().GetStringAsync(url);
        var obj = JsonSerializer.Deserialize<Result<MyFavoriteDto>>(json);
        if (obj == null)
        {
            return new Result<MyFavoriteDto>(999, "连接服务器失败", null);
        }

        return obj;
    }

    public async Task<Result> RemoveAsync(int id)
    {
        var url = string.Format(GlobalConfig.ApiSetting.MyFavorite.Remove, id);
        var json = await HttpClientSingleton.Instance().PostReadAsStringAsync(url, id);
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null)
        {
            return new Result(999, "连接服务器失败");
        }
        return obj;
    }
}