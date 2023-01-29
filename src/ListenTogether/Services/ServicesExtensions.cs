using ListenTogether.Business.Services;
using ListenTogether.Services.MusicSwitchServer;

namespace ListenTogether.Services;

internal static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IMusicSwitchServer, MusicSwitchRepeatListServer>();
        builder.Services.AddSingleton<IMusicSwitchServer, MusicSwitchRepeatOneServer>();
        builder.Services.AddSingleton<IMusicSwitchServer, MusicSwitchShuffleServer>();
        builder.Services.AddSingleton<IMusicSwitchServerFactory, MusicSwitchServerFactory>();

        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddTransient<WifiOptionsService>();
        builder.Services.AddSingleton<PlayerService>();
        builder.Services.AddSingleton<MusicPlayerService>();
        builder.Services.AddSingleton<MusicResultService>(); 
        builder.Services.AddSingleton<UserFavoriteService>();
        return builder;
    }
}