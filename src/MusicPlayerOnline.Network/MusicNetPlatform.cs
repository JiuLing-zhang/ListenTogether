using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Model.Network;
using MusicPlayerOnline.Network.BuildMusicDetail;
using MusicPlayerOnline.Network.SearchMusic;
using MusicPlayerOnline.Network.UpdateMusicDetail;

namespace MusicPlayerOnline.Network;
public class MusicNetPlatform
{
    //搜索链
    private readonly SearchAbstract _netEaseSearcher = new NetEaseSearcher(PlatformEnum.NetEase);
    private readonly SearchAbstract _kuGouSearcher = new KuGouSearcher(PlatformEnum.KuGou);
    private readonly SearchAbstract _miGuSearcher = new MiGuSearcher(PlatformEnum.MiGu);

    //构建详情链
    private readonly BuildAbstract _netEaseBuilder = new NetEaseBuilder(PlatformEnum.NetEase);
    private readonly BuildAbstract _kuGouBuilder = new KuGouBuilder(PlatformEnum.KuGou);
    private readonly BuildAbstract _miGuBuilder = new MiGuBuilder(PlatformEnum.MiGu);

    //更新歌曲信息
    private readonly UpdateAbstract _netEaseUpdater = new NetEaseUpdater(PlatformEnum.NetEase);
    public MusicNetPlatform()
    {
        //搜索
        _miGuSearcher.SetNextHandler(_netEaseSearcher);
        _netEaseSearcher.SetNextHandler(_kuGouSearcher);

        //详情
        _netEaseBuilder.SetNextHandler(_kuGouBuilder);
        _kuGouBuilder.SetNextHandler(_miGuBuilder);
    }

    public async Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword)
    {
        return await _miGuSearcher.Search(platform, keyword);
    }

    public async Task<Music?> BuildMusicDetail(MusicSearchResult music)
    {
        return await _netEaseBuilder.Build(music);
    }

    public async Task<Music?> UpdateMusicDetail(Music music)
    {
        return await _netEaseUpdater.Update(music);
    }
}