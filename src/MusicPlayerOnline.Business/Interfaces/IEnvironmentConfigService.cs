using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;

public interface IEnvironmentConfigService
{
    EnvironmentSetting ReadAllSettings();
    void WritePlayerSetting(PlayerSetting playerSetting);
}
