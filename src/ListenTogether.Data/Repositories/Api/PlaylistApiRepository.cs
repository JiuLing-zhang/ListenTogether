using ListenTogether.Data.Interfaces;
using ListenTogether.Model;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Data.Repositories.Api;
public class PlaylistApiRepository : IPlaylistRepository
{
    public async Task<bool> AddOrUpdateAsync(Playlist playlist)
    {
        var requestPlaylist = new PlaylistRequest()
        {
            PlatformName = playlist.PlatformName,
            MusicId = playlist.MusicId,
            MusicName = playlist.MusicName,
            MusicArtist = playlist.MusicArtist,
            MusicAlbum = playlist.MusicAlbum
        };

        string content = requestPlaylist.ToJson();
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithToken.PostAsync(DataConfig.ApiSetting.Playlist.AddOrUpdate, sc);
        var json = await response.Content.ReadAsStringAsync();
        var obj = json.ToObject<Result>();
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<List<Playlist>?> GetAllAsync()
    {
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(DataConfig.ApiSetting.Playlist.GetAll);
        var obj = json.ToObject<List<PlaylistResponse>>();
        if (obj == null)
        {
            return default;
        }

        return obj.Select(x => new Playlist()
        {
            Id = x.Id,
            PlatformName = x.PlatformName,
            MusicId = x.MusicId,
            MusicArtist = x.MusicArtist,
            MusicName = x.MusicName,
            MusicAlbum = x.MusicAlbum
        }).ToList();
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var url = string.Format(DataConfig.ApiSetting.Playlist.Remove, id);
        var response = await DataConfig.HttpClientWithToken.PostAsync(url, null);
        var json = await response.Content.ReadAsStringAsync();
        var obj = json.ToObject<Result>();
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> RemoveAllAsync()
    {
        var response = await DataConfig.HttpClientWithToken.PostAsync(DataConfig.ApiSetting.Playlist.RemoveAll, null);
        var json = await response.Content.ReadAsStringAsync();
        var obj = json.ToObject<Result>();
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }
}