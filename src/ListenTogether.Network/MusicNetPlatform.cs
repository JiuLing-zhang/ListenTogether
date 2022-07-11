using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.BuildMusicDetail;
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

    private readonly IMusicProvider _kuGouMusicProvider = new KuGouMusicProvider();
    private readonly IMusicProvider _netEaseMusicProvider = new NetEaseMusicProvider();
    public MusicNetPlatform()
    {
        //搜索
        _miGuSearcher.SetNextHandler(_netEaseSearcher);
        _netEaseSearcher.SetNextHandler(_kuWoSearcher);
        _kuWoSearcher.SetNextHandler(_kuGouSearcher);
    }

    public async Task<List<string>> GetSearchSuggest(string keyword)
    {
        return await _netEaseMusicProvider.GetSearchSuggest(keyword);
    }

    public async Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword)
    {
        return await _miGuSearcher.Search(platform, keyword);
    }

    public async Task<Music?> GetMusicDetail(MusicSearchResult music)
    {
        return await MusicBuilderFactory.Create(music.Platform).GetMusicDetail(music);
    }

    public async Task<Music?> UpdatePlayUrl(Music music)
    {
        switch (music.Platform)
        {
            case PlatformEnum.NetEase:
                return await _netEaseMusicProvider.UpdatePlayUrl(music);
            case PlatformEnum.KuGou:
                return await _kuGouMusicProvider.UpdatePlayUrl(music);
            case PlatformEnum.MiGu:
                break;
            default:
                break;
        }
        throw new ArgumentException("当前平台无需更新地址");
    }
}