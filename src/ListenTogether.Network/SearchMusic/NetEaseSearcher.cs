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

    public override async Task<List<MusicResultShow>> DoSearchAsync(string keyword, List<MusicResultShow> allResult)
    {
        var musics = await _myMusicProvider.SearchAsync(keyword);
        allResult.AddRange(musics);
        return allResult;
    }
}
