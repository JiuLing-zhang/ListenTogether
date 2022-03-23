using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
internal class MusicServiceFactory
{
    public static IMusicService Create()
    {
        if (GlobalConfig.IsLogin)
        {
            var apiHttpMessageHandler = new ApiHttpMessageHandler(LocalTokenServiceFactory.Create());
            return new MusicApiService(new HttpClientProvider(apiHttpMessageHandler));
        }

        return new MusicLocalService();
    }
}