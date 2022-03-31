using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;

public interface IEnvironmentConfigRepository
{
    EnvironmentSetting ReadAllSettings();
    void WritePlayerSetting(PlayerSetting playerSetting);

    bool WriteGeneralSetting(GeneralSetting generalSetting);

    bool WriteSearchSetting(SearchSetting searchSetting);

    bool WritePlaySetting(PlaySetting playSetting);
}
