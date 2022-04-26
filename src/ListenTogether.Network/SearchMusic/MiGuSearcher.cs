using MusicPlayerOnline.EasyLog;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.SearchMusic;
public class MiGuSearcher : SearchAbstract
{
    private readonly IMusicProvider _myMusicProvider;
    public MiGuSearcher(PlatformEnum platform) : base(platform)
    {
        _myMusicProvider = new MiGuMusicProvider();
    }

    public override async Task<List<MusicSearchResult>> DoSearch(string keyword, List<MusicSearchResult> allResult)
    {
        var (isSucceed, _, musics) = await _myMusicProvider.Search(keyword);
        if (isSucceed == false || musics == null)
        {
            Logger.Info($"搜索咪咕歌曲失败，关键字：{keyword}");
            return allResult;
        }
        allResult.AddRange(musics);
        return allResult;
    }
}
