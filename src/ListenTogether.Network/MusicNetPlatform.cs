using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.MusicProvider;
using ListenTogether.Network.SearchMusic;

namespace ListenTogether.Network;
public class MusicNetPlatform
{
    //搜索链
    private readonly SearchAbstract _netEaseSearcher = new NetEaseSearcher();
    private readonly SearchAbstract _kuGouSearcher = new KuGouSearcher();
    private readonly SearchAbstract _miGuSearcher = new MiGuSearcher();
    private readonly SearchAbstract _kuWoSearcher = new KuWoSearcher();

    public MusicNetPlatform()
    {
        //搜索
        _miGuSearcher.SetNextHandler(_kuWoSearcher);
        _kuWoSearcher.SetNextHandler(_netEaseSearcher);
        _netEaseSearcher.SetNextHandler(_kuGouSearcher);
    }

    public void SetMusicFormatType(MusicFormatTypeEnum musicFormatType)
    {
        MusicProviderSetting.MusicFormatType = musicFormatType;
    }

    public async Task<List<string>?> GetHotWordAsync()
    {
        return await MusicProviderFactory.Create(PlatformEnum.KuWo).GetHotWordAsync();
    }

    public async Task<List<string>?> GetSearchSuggestAsync(string keyword)
    {
        return await MusicProviderFactory.Create(PlatformEnum.NetEase).GetSearchSuggestAsync(keyword);
    }

    public async Task<List<MusicResultShow>> SearchAsync(PlatformEnum platform, string keyword)
    {
        return await _miGuSearcher.SearchAsync(platform, keyword);
    }

    public async Task<string> GetPlayUrlAsync(PlatformEnum platform, string id, string extendDataJson = "")
    {
        return await MusicProviderFactory.Create(platform).GetPlayUrlAsync(id, extendDataJson);
    }
    public async Task<string> GetImageUrlAsync(PlatformEnum platform, string id, string extendDataJson = "")
    {
        return await MusicProviderFactory.Create(platform).GetImageUrlAsync(id, extendDataJson);
    }

    public async Task<string?> GetLyricAsync(PlatformEnum platform, string id, string extendDataJson = "")
    {
        return await MusicProviderFactory.Create(platform).GetLyricAsync(id, extendDataJson);
    }

    public Task<string> GetPlayPageUrlAsync(PlatformEnum platform, string id, string extendDataJson = "")
    {
        return MusicProviderFactory.Create(platform).GetShareUrlAsync(id, extendDataJson);
    }

    public async Task<(List<MusicTag> HotTags, List<MusicTypeTag> AllTypes)> GetMusicTagsAsync(PlatformEnum platform)
    {
        return await MusicProviderFactory.Create(platform).GetMusicTagsAsync();
    }

    public async Task<List<SongMenu>> GetSongMenusFromTagAsync(PlatformEnum platform, string id, int page)
    {
        return await MusicProviderFactory.Create(platform).GetSongMenusFromTagAsync(id, page);
    }
    public Task<List<SongMenu>> GetSongMenusFromTop(PlatformEnum platform)
    {
        return MusicProviderFactory.Create(platform).GetSongMenusFromTop();
    }

    public async Task<List<MusicResultShow>> GetTopMusicsAsync(PlatformEnum platform, string topId)
    {
        return await MusicProviderFactory.Create(platform).GetTopMusicsAsync(topId);
    }

    public async Task<List<MusicResultShow>> GetTagMusicsAsync(PlatformEnum platform, string tagId)
    {
        return await MusicProviderFactory.Create(platform).GetTagMusicsAsync(tagId);
    }
}