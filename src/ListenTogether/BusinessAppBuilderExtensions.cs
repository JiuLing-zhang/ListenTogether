using ListenTogether.Data.Api.Repositories;
using ListenTogether.Data.Maui;
using ListenTogether.Network;

namespace ListenTogether;

public static class AppBuilderExtensions
{
    public static MauiAppBuilder UseBusiness(this MauiAppBuilder builder)
    {
        //网络数据平台
        builder.Services.AddSingleton<MusicNetPlatform>();
        //本地服务
        builder.Services.AddSingleton<IEnvironmentConfigService, EnvironmentConfigService>();
        builder.Services.AddSingleton<IPlaylistService, PlaylistService>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IMyFavoriteService, MyFavoriteService>();
        builder.Services.AddSingleton<IMusicService, MusicService>();

        return builder;
    }
}
