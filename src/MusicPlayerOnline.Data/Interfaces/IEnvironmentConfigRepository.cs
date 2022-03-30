using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;

public interface IEnvironmentConfigRepository
{
    EnvironmentSetting ReadAllSettings();
    void WritePlayerSetting(PlayerSetting playerSetting);
}
