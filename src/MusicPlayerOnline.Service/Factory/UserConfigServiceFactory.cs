using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
internal class UserConfigServiceFactory
{
    public static IUserConfigService Create()
    {
        if (GlobalConfig.IsLogin)
        {
            return new UserConfigApiService();
        }

        return new UserConfigLocalService();
    }
}