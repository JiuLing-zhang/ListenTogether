using ListenTogether.Business.Interfaces;
using ListenTogether.Business.Services;
using ListenTogether.Data.Interfaces;
using ListenTogether.Data.Repositories.Api;
using ListenTogether.Data.Repositories.Local;
using ListenTogether.Network;
using ListenTogether.Pages;
using ListenTogether.Pages.Services;
using ListenTogether.Services.MusicSwitchServer;

namespace ListenTogetherMauiBlazor;
internal static class BusinessServicesExtensions
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddSingleton<IMusicSwitchServer, MusicSwitchRepeatListServer>();
        services.AddSingleton<IMusicSwitchServer, MusicSwitchRepeatOneServer>();
        services.AddSingleton<IMusicSwitchServer, MusicSwitchShuffleServer>();
        services.AddSingleton<IMusicSwitchServerFactory, MusicSwitchServerFactory>();

        services.AddSingleton<HttpClient>();
        services.AddTransient<IWifiOptionsService, WifiOptionsService>();
        services.AddSingleton<IPlayerService, PlayerService>();
        services.AddSingleton<MusicPlayerService>();
        services.AddSingleton<MusicResultService>();


        //网络数据平台
        services.AddSingleton<MusicNetPlatform>();
        //本地服务
        services.AddSingleton<IEnvironmentConfigService, EnvironmentConfigService>();
        services.AddSingleton<IMusicNetworkService, MusicNetworkService>();
        services.AddSingleton<IPlaylistService, PlaylistService>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IMusicCacheService, MusicCacheService>();
        services.AddSingleton<IMyFavoriteService, MyFavoriteService>();
        services.AddSingleton<IMusicService, MusicService>();

        //数据服务
        services.AddSingleton<IEnvironmentConfigRepository, EnvironmentConfigLocalRepository>();
        services.AddSingleton<IMusicRepository, MusicApiRepository>();
        services.AddSingleton<IMyFavoriteRepository, MyFavoriteApiRepository>();
        services.AddSingleton<IPlaylistRepository, PlaylistLocalRepository>();
        services.AddSingleton<IUserApiRepository, UserApiRepository>();
        services.AddSingleton<IMusicCacheRepository, MusicCacheRepository>();
        return services;
    }
}