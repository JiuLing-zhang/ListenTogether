using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
internal class MyFavoriteServiceFactory
{
    public static IMyFavoriteService Create()
    {
        if (GlobalConfig.IsLogin)
        {
            var apiHttpMessageHandler = new ApiHttpMessageHandler(LocalTokenServiceFactory.Create());
            return new MyFavoriteApiService(new HttpClientProvider(apiHttpMessageHandler));
        }

        return new MyFavoriteLocalService();
    }
}