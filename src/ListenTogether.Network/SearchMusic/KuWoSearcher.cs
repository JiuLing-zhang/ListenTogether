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

    public override async Task<List<MusicResultShow>> DoSearchAsync(string keyword, List<MusicResultShow> allResult)
    {
        var musics = await _myMusicProvider.SearchAsync(keyword);
        allResult.AddRange(musics);
        return allResult;
    }
}
