using ListenTogether.Model.Enums;

namespace ListenTogether.Services.MusicSwitchServer;
public interface IMusicSwitchServerFactory
{
    public IMusicSwitchServer Create(PlayModeEnum playMode);
}
