using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.MusicProvider;

namespace ListenTogether.Network.SearchMusic;
public class MiGuSearcher : SearchAbstract
{
    private readonly IMusicProvider _myMusicProvider;
    public MiGuSearcher() : base(PlatformEnum.MiGu)
    {
        _myMusicProvider = new MiGuMusicProvider();
    }

    public override async Task<List<MusicResultShow>> DoSearchAsync(string keyword, List<MusicResultShow> allResult)
    {
        var musics = await _myMusicProvider.SearchAsync(keyword);
        allResult.AddRange(musics);
        return allResult;
    }
}
