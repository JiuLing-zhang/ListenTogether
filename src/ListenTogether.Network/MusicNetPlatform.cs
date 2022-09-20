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

    public async Task<List<string>?> GetHotWordAsync()
    {
        return await MusicProviderFactory.Create(PlatformEnum.KuWo).GetHotWordAsync();
    }

    public async Task<List<string>?> GetSearchSuggestAsync(string keyword)
    {
        return await MusicProviderFactory.Create(PlatformEnum.NetEase).GetSearchSuggestAsync(keyword);
    }

    public async Task<List<MusicSearchResult>> SearchAsync(PlatformEnum platform, string keyword)
    {
        return await _miGuSearcher.SearchAsync(platform, keyword);
    }

    public async Task<Music?> GetDetailAsync(MusicSearchResult music, MusicFormatTypeEnum musicFormatType)
    {
        return await MusicProviderFactory.Create(music.Platform).GetDetailAsync(music, musicFormatType);
    }

    public async Task<string?> GetPlayUrlAsync(Music music, MusicFormatTypeEnum musicFormatType)
    {
        return await MusicProviderFactory.Create(music.Platform).GetPlayUrlAsync(music, musicFormatType);
    }

    public Task<string> GetMusicPlayPageUrlAsync(Music music)
    {
        return MusicProviderFactory.Create(music.Platform).GetMusicShareUrlAsync(music);
    }
}