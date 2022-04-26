using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.BuildMusicDetail;
using ListenTogether.Network.MusicProvider;
using ListenTogether.Network.SearchMusic;

namespace ListenTogether.Network;
public class MusicNetPlatform
{
    //搜索链
    private readonly SearchAbstract _netEaseSearcher = new NetEaseSearcher(PlatformEnum.NetEase);
    private readonly SearchAbstract _kuGouSearcher = new KuGouSearcher(PlatformEnum.KuGou);
    private readonly SearchAbstract _miGuSearcher = new MiGuSearcher(PlatformEnum.MiGu);

    public MusicNetPlatform()
    {
        //搜索
        _miGuSearcher.SetNextHandler(_netEaseSearcher);
        _netEaseSearcher.SetNextHandler(_kuGouSearcher);
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
        if (music.Platform != PlatformEnum.NetEase)
        {
            throw new ArgumentException("当前平台无需更新地址");
        }

        IMusicProvider musicProvider = new NetEaseMusicProvider();
        return await musicProvider.UpdatePlayUrl(music);
    }
}