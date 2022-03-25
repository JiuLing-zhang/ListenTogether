using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
public class MusicServiceFactory
{
    public static IMusicService Create()
    {
        if (GlobalConfig.IsUseApiInterface)
        {
            var apiHttpMessageHandler = new ApiHttpMessageHandler(LocalTokenServiceFactory.Create());
            return new MusicApiService(new HttpClientProvider(apiHttpMessageHandler));
        }

        return new MusicLocalService();
    }
}