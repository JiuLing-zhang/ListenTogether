using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
public class LocalTokenServiceFactory
{
    public static ILocalTokenService Create()
    {
        return new LocalTokenService();
    }
}