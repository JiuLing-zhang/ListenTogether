using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
public class PlaylistServiceFactory
{
    public static IPlaylistService Create()
    {
        if (GlobalConfig.IsUseApiInterface)
        {
            var apiHttpMessageHandler = new ApiHttpMessageHandler(LocalTokenServiceFactory.Create());
            return new PlaylistApiService(new HttpClientProvider(apiHttpMessageHandler));
        }

        return new PlaylistLocalService();
    }
}