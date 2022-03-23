using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
internal class UserConfigServiceFactory
{
    public static IUserConfigService Create()
    {
        if (GlobalConfig.IsLogin)
        {
            var apiHttpMessageHandler = new ApiHttpMessageHandler(LocalTokenServiceFactory.Create());
            return new UserConfigApiService(new HttpClientProvider(apiHttpMessageHandler));
        }

        return new UserConfigLocalService();
    }
}