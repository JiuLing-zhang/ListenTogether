using ListenTogether.Model.Enums;

namespace ListenTogether.Services.MusicSwitchServer;

public class MusicSwitchServerFactory : IMusicSwitchServerFactory
{
    private readonly IEnumerable<IMusicSwitchServer> _services;
    public MusicSwitchServerFactory(IEnumerable<IMusicSwitchServer> services)
    {
        _services = services;
    }

    public IMusicSwitchServer Create(PlayModeEnum playMode)
    {
        string serverName;
        switch (playMode)
        {
            case PlayModeEnum.RepeatOne:
                serverName = nameof(MusicSwitchRepeatOneServer);
                break;
            case PlayModeEnum.RepeatList:
                serverName = nameof(MusicSwitchRepeatListServer);
                break;
            case PlayModeEnum.Shuffle:
                serverName = nameof(MusicSwitchShuffleServer);
                break;
            default:
                throw new ArgumentException("不支持的播放方式");
        }
        return _services.First(x => x.GetType().Name == serverName);
    }
}