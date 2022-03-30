using Microsoft.Extensions.DependencyInjection;
using MusicPlayerOnline.Business.Factories;
using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Business.Services;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Data.Repositories.Api;
using MusicPlayerOnline.Data.Repositories.Local;
using MusicPlayerOnline.Network;

namespace MusicPlayerOnline.Business;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        //网络数据平台
        services.AddSingleton<MusicNetPlatform>();

        //本地、远程服务工厂
        services.AddSingleton<IMusicServiceFactory, MusicServiceFactory>();
        services.AddSingleton<IMyFavoriteServiceFactory, MyFavoriteServiceFactory>();
        services.AddSingleton<IPlaylistServiceFactory, PlaylistServiceFactory>();
        services.AddSingleton<IUserConfigServiceFactory, UserConfigServiceFactory>();

        //本地服务
        services.AddSingleton<IEnvironmentConfigService, EnvironmentConfigService>();
        services.AddSingleton<IMusicNetworkService, MusicNetworkService>();
        services.AddSingleton<IUserService, UserService>();

        //数据服务
        services.AddSingleton<IEnvironmentConfigRepository, EnvironmentConfigLocalRepository>();
        services.AddSingleton<IMusicRepository, MusicApiRepository>();
        services.AddSingleton<IMusicRepository, MusicLocalRepository>();
        services.AddSingleton<IMyFavoriteRepository, MyFavoriteApiRepository>();
        services.AddSingleton<IMyFavoriteRepository, MyFavoriteLocalRepository>();
        services.AddSingleton<IPlaylistRepository, PlaylistApiRepository>();
        services.AddSingleton<IPlaylistRepository, PlaylistLocalRepository>();
        services.AddSingleton<ITokenRepository, TokenLocalRepository>();     
        services.AddSingleton<IUserConfigRepository, UserConfigLocalRepository>();
        services.AddSingleton<IUserRepository, UserApiRepository>();

        return services;
    }

}
