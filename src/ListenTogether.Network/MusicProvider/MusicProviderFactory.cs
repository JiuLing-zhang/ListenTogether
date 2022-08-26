using ListenTogether.Model.Enums;

namespace ListenTogether.Network.MusicProvider;
internal class MusicProviderFactory
{
    public static IMusicProvider Create(PlatformEnum platform)
    {
        return platform switch
        {
            PlatformEnum.NetEase => new NetEaseMusicProvider(),
            PlatformEnum.KuGou => new KuGouMusicProvider(),
            PlatformEnum.MiGu => new MiGuMusicProvider(),
            PlatformEnum.KuWo => new KuWoMusicProvider(),
            _ => throw new ArgumentException("歌曲构建器生成失败：不支持的平台")
        };
    }
}