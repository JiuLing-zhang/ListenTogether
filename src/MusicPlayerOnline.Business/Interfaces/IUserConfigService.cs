using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;
public interface IUserConfigService
{
    UserSetting? ReadAllSettings();
    bool WriteGeneralSetting(GeneralSetting generalSetting);

    bool WriteSearchSetting(SearchSetting searchSetting);

    bool WritePlaySetting(PlaySetting playSetting);
}