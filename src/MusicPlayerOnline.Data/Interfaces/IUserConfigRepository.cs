using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;
public interface IUserConfigRepository
{
    UserSetting ReadAllSettings();
    bool WriteGeneralSetting(GeneralSetting generalSetting);

    bool WriteSearchSetting(SearchSetting searchSetting);

    bool WritePlaySetting(PlaySetting playSetting);
}