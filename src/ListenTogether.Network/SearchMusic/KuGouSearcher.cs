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

    public override async Task<List<MusicResultShow>> DoSearchAsync(string keyword, List<MusicResultShow> allResult)
    {
        var musics = await _myMusicProvider.SearchAsync(keyword);
        allResult.AddRange(musics);
        return allResult;
    }
}