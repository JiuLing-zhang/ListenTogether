using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
internal class PlaylistServiceFactory
{
    public static IPlaylistService Create()
    {
        if (GlobalConfig.IsLogin)
        {
            return new PlaylistApiService();
        }

        return new PlaylistLocalService();
    }
}