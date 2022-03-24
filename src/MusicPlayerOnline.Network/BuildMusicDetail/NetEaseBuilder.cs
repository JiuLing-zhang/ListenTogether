using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Model.Network;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.BuildMusicDetail;
public class NetEaseBuilder : BuildAbstract
{
    private readonly IMusicProvider _myMusicProvider;
    public NetEaseBuilder(PlatformEnum platform) : base(platform)
    {
        _myMusicProvider = new NetEaseMusicProvider();
    }
    public override async Task<Music?> DoBuild(MusicSearchResult music)
    {
        return await _myMusicProvider.GetMusicDetail(music);
    }
}