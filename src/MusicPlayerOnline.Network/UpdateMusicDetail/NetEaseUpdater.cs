using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Model.Network;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.UpdateMusicDetail;
public class NetEaseUpdater : UpdateAbstract
{
    private readonly IMusicProvider _myMusicProvider;
    public NetEaseUpdater(PlatformEnum platform) : base(platform)
    {
        _myMusicProvider = new NetEaseMusicProvider();
    }

    public override async Task<Music?> DoUpdate(Music music)
    {
        return await _myMusicProvider.UpdateMusicDetail(music);
    }
}