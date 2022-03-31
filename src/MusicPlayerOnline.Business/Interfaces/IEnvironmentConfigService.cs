using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;

public interface IEnvironmentConfigService
{
    EnvironmentSetting ReadAllSettings();
    void WritePlayerSetting(PlayerSetting playerSetting);
    bool WriteGeneralSetting(GeneralSetting generalSetting);

    bool WriteSearchSetting(SearchSetting searchSetting);

    bool WritePlaySetting(PlaySetting playSetting);
}
