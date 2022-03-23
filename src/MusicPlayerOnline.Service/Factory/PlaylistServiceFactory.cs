using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
internal class PlaylistServiceFactory
{
    public static IPlaylistService Create()
    {
        if (GlobalConfig.IsLogin)
        {
            var apiHttpMessageHandler = new ApiHttpMessageHandler(LocalTokenServiceFactory.Create());
            return new PlaylistApiService(new HttpClientProvider(apiHttpMessageHandler));
        }

        return new PlaylistLocalService();
    }
}