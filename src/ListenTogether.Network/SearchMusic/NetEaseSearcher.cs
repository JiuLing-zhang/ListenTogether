using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.MusicProvider;

namespace ListenTogether.Network.SearchMusic;
public class NetEaseSearcher : SearchAbstract
{
    private readonly IMusicProvider _myMusicProvider;
    public NetEaseSearcher() : base(PlatformEnum.NetEase)
    {
        _myMusicProvider = new NetEaseMusicProvider();
    }

    public override async Task<List<MusicSearchResult>> DoSearchAsync(string keyword, List<MusicSearchResult> allResult)
    {
        var (isSucceed, _, musics) = await _myMusicProvider.SearchAsync(keyword);
        if (isSucceed == false || musics == null)
        {
            Logger.Info($"搜索网易歌曲失败，关键字：{keyword}");
            return allResult;
        }

        allResult.AddRange(musics);
        return allResult;
    }
}
