using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;
using MusicPlayerOnline.Service.Services;

namespace MusicPlayerOnline.Service.Factory;
public class UserConfigServiceFactory
{
    public static IUserConfigService Create()
    {
        if (GlobalConfig.IsUseApiInterface)
        {
            var apiHttpMessageHandler = new ApiHttpMessageHandler(LocalTokenServiceFactory.Create());
            return new UserConfigApiService(new HttpClientProvider(apiHttpMessageHandler));
        }

        return new UserConfigLocalService();
    }
}