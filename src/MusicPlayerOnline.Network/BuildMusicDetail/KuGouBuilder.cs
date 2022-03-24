using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Model.Network;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.BuildMusicDetail;
public class KuGouBuilder : BuildAbstract
{
    private readonly IMusicProvider _myMusicProvider;
    public KuGouBuilder(PlatformEnum platform) : base(platform)
    {
        _myMusicProvider = new KuGouMusicProvider();
    }

    public override async Task<Music?> DoBuild(MusicSearchResult music)
    {
        return await _myMusicProvider.GetMusicDetail(music);
    }
}