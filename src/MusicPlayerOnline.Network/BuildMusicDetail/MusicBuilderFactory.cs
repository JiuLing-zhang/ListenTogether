using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.BuildMusicDetail;
public class MusicBuilderFactory
{
    public static IMusicProvider Create(PlatformEnum platform)
    {
        return platform switch
        {
            PlatformEnum.NetEase => new NetEaseMusicProvider(),
            PlatformEnum.KuGou => new KuGouMusicProvider(),
            PlatformEnum.MiGu => new MiGuMusicProvider(),
            _ => throw new ArgumentException("歌曲构建器生成失败：不支持的平台")
        };
    }
}