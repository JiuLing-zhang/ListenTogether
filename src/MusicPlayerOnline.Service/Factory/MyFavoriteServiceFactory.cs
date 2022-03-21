using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
internal class MyFavoriteServiceFactory
{
    public static IMyFavoriteService Create()
    {
        if (GlobalConfig.IsLogin)
        {
            return new MyFavoriteApiService();
        }

        return new MyFavoriteLocalService();
    }
}