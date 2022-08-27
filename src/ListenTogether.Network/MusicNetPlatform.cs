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

    public async Task<List<string>?> GetHotWord()
    {
        return await MusicProviderFactory.Create(PlatformEnum.KuWo).GetHotWord();
    }

    public async Task<List<string>> GetSearchSuggest(string keyword)
    {
        return await MusicProviderFactory.Create(PlatformEnum.NetEase).GetSearchSuggest(keyword);
    }

    public async Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword)
    {
        return await _miGuSearcher.Search(platform, keyword);
    }

    public async Task<Music?> GetMusicDetail(MusicSearchResult music)
    {
        return await MusicProviderFactory.Create(music.Platform).GetMusicDetail(music);
    }

    public async Task<Music?> UpdatePlayUrl(Music music)
    {
        return await MusicProviderFactory.Create(music.Platform).UpdatePlayUrl(music);
    }

    public Task<string> GetMusicPlayPageUrl(Music music)
    {
        return MusicProviderFactory.Create(music.Platform).GetMusicShareUrl(music);
    }
}