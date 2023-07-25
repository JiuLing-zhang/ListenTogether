using ListenTogether.Data.Api.Repositories;
using ListenTogether.Data.Interface;
using ListenTogether.Data.Maui;
using ListenTogether.Pages;
using ListenTogether.Pages.Services;
using ListenTogether.Services.MusicSwitchServer;
using NetMusicLib;

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

        //数据服务
        services.AddSingleton<IEnvironmentConfigService, EnvironmentConfigService>();
        services.AddSingleton<IMusicService, MusicService>();
        services.AddSingleton<IMyFavoriteService, MyFavoriteService>();
        services.AddSingleton<IPlaylistService, PlaylistService>();
        services.AddSingleton<IUserService, UserService>();
        return services;
    }
}