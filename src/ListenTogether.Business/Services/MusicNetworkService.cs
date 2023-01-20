using ListenTogether.Business.Interfaces;
using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network;

namespace ListenTogether.Business.Services;

public class MusicNetworkService : IMusicNetworkService
{
    private readonly MusicNetPlatform _musicNetPlatform;
    public MusicNetworkService(MusicNetPlatform musicNetPlatform)
    {
        _musicNetPlatform = musicNetPlatform;
    }

    public async Task<List<string>?> GetSearchSuggestAsync(string keyword)
    {
        return await _musicNetPlatform.GetSearchSuggestAsync(keyword);
    }

    public async Task<List<MusicResultShow>> SearchAsync(PlatformEnum platform, string keyword)
    {
        return await _musicNetPlatform.SearchAsync(platform, keyword);
    }

    public async Task<string> GetPlayUrlAsync(PlatformEnum platform, string id, object? extendData = null)
    {
        return await _musicNetPlatform.GetPlayUrlAsync(platform, id, extendData);
    }
    public async Task<string?> GetLyricAsync(PlatformEnum platform, string id, object? extendData = null)
    {
        return await _musicNetPlatform.GetLyricAsync(platform, id, extendData);
    }
    public Task<string> GetPlayPageUrlAsync(PlatformEnum platform, string id, object? extendData = null)
    {
        return _musicNetPlatform.GetPlayPageUrlAsync(platform, id, extendData);
    }

    public async Task<List<string>?> GetHotWordAsync()
    {
        return await _musicNetPlatform.GetHotWordAsync();
    }

    public async Task<(List<MusicTag> HotTags, List<MusicTypeTag> AllTypes)> GetMusicTagsAsync(PlatformEnum platform)
    {
        return await _musicNetPlatform.GetMusicTagsAsync(platform);
    }

    public async Task<List<SongMenu>> GetSongMenusFromTagAsync(PlatformEnum platform, string id)
    {
        return await _musicNetPlatform.GetSongMenusFromTagAsync(platform, id);
    }

    public Task<List<SongMenu>> GetSongMenusFromTop(PlatformEnum platform)
    {
        return _musicNetPlatform.GetSongMenusFromTop(platform);
    }

    public async Task<List<MusicResultShow>> GetTopMusicsAsync(PlatformEnum platform, string topId)
    {
        return await _musicNetPlatform.GetTopMusicsAsync(platform, topId);
    }

    public async Task<List<MusicResultShow>> GetTagMusicsAsync(PlatformEnum platform, string tagId)
    {
        return await _musicNetPlatform.GetTagMusicsAsync(platform, tagId);
    }

}