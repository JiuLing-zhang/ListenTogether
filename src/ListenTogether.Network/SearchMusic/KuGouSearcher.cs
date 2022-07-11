using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.MusicProvider;

namespace ListenTogether.Network.SearchMusic;
public class KuGouSearcher : SearchAbstract
{
    private readonly IMusicProvider _myMusicProvider;
    public KuGouSearcher() : base(PlatformEnum.KuGou)
    {
        _myMusicProvider = new KuGouMusicProvider();
    }

    public override async Task<List<MusicSearchResult>> DoSearch(string keyword, List<MusicSearchResult> allResult)
    {
        var (isSucceed, _, musics) = await _myMusicProvider.Search(keyword);
        if (isSucceed == false || musics == null)
        {
            Logger.Info($"搜索酷狗歌曲失败，关键字：{keyword}");
            return allResult;
        }

        allResult.AddRange(musics);
        return allResult;
    }
}