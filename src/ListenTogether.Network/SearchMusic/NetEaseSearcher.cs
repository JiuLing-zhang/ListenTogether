using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.MusicProvider;

namespace ListenTogether.Network.SearchMusic;
public class NetEaseSearcher : SearchAbstract
{
    private readonly IMusicProvider _myMusicProvider;
    public NetEaseSearcher(PlatformEnum platform) : base(platform)
    {
        _myMusicProvider = new NetEaseMusicProvider();
    }

    public override async Task<List<MusicSearchResult>> DoSearch(string keyword, List<MusicSearchResult> allResult)
    {
        var (isSucceed, _, musics) = await _myMusicProvider.Search(keyword);
        if (isSucceed == false || musics == null)
        {
            Logger.Info($"搜索网易歌曲失败，关键字：{keyword}");
            return allResult;
        }

        allResult.AddRange(musics);
        return allResult;
    }
}
