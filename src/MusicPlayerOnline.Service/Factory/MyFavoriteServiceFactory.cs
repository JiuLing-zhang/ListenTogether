using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
public class MyFavoriteServiceFactory
{
    public static IMyFavoriteService Create()
    {
        if (GlobalConfig.IsUseApiInterface)
        {
            var apiHttpMessageHandler = new ApiHttpMessageHandler(LocalTokenServiceFactory.Create());
            return new MyFavoriteApiService(new HttpClientProvider(apiHttpMessageHandler));
        }

        return new MyFavoriteLocalService();
    }
}