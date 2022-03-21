using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
internal class MusicServiceFactory
{
    public static IMusicService Create()
    {
        if (GlobalConfig.IsLogin)
        {
            return new MusicApiService();
        }

        return new MusicLocalService();
    }
}