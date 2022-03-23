using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
public class UserServiceFactory
{
    public static IUserService Create()
    {
        var apiHttpMessageHandler = new ApiHttpMessageHandler(LocalTokenServiceFactory.Create());
        return new UserService(new HttpClientProvider(apiHttpMessageHandler));
    }
}