using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Model.Network;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.BuildMusicDetail;
public class MiGuBuilder : BuildAbstract
{
    private readonly IMusicProvider _myMusicProvider;
    public MiGuBuilder(PlatformEnum platform) : base(platform)
    {
        _myMusicProvider = new MiGuMusicProvider();
    }

    public override async Task<Music?> DoBuild(MusicSearchResult music)
    {
        return await _myMusicProvider.GetMusicDetail(music);
    }
}