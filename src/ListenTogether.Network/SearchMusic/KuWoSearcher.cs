using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.MusicProvider;

namespace ListenTogether.Network.SearchMusic;
internal class KuWoSearcher : SearchAbstract
{
    private readonly IMusicProvider _myMusicProvider;
    public KuWoSearcher() : base(PlatformEnum.KuWo)
    {
        _myMusicProvider = new KuWoMusicProvider();
    }

    public override async Task<List<MusicSearchResult>> DoSearch(string keyword, List<MusicSearchResult> allResult)
    {
        var (isSucceed, _, musics) = await _myMusicProvider.Search(keyword);
        if (isSucceed == false || musics == null)
        {
            Logger.Info($"搜索酷我歌曲失败，关键字：{keyword}");
            return allResult;
        }

        allResult.AddRange(musics);
        return allResult;
    }
}
