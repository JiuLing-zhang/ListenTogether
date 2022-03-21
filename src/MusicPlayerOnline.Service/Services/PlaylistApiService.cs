using System.Text.Json;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service.Services;
internal class PlaylistApiService : IPlaylistService
{
    public async Task<Result> AddOrUpdateAsync(Playlist playlist)
    {
        var json = await HttpClientSingleton.Instance().PostReadAsStringAsync(GlobalConfig.ApiSetting.Playlist.AddOrUpdate, playlist);
        var obj = JsonSerializer.Deserialize<Result>(json);
        return obj ?? new Result(999, "连接服务器失败");
    }

    public async Task<List<PlaylistDto>?> GetAllAsync()
    {
        var json = await HttpClientSingleton.Instance().GetStringAsync(GlobalConfig.ApiSetting.Playlist.GetAll);
        var obj = JsonSerializer.Deserialize<List<PlaylistDto>>(json);
        return obj ?? default;
    }

    public async Task<Result> RemoveAsync(int id)
    {
        var url = string.Format(GlobalConfig.ApiSetting.Playlist.Remove, id);
        var json = await HttpClientSingleton.Instance().PostStringAsync(url);
        var obj = JsonSerializer.Deserialize<Result>(json);
        return obj ?? new Result(999, "连接服务器失败");
    }

    public async Task<Result> RemoveAllAsync()
    {
        var json = await HttpClientSingleton.Instance().PostStringAsync(GlobalConfig.ApiSetting.Playlist.RemoveAll);
        var obj = JsonSerializer.Deserialize<Result>(json);
        return obj ?? new Result(999, "连接服务器失败");
    }
}