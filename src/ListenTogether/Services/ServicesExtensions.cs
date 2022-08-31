using JiuLing.CommonLibs.Net;
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

        builder.Services.AddTransient<WifiOptionsService>();
        builder.Services.AddSingleton<PlayerService>();
        builder.Services.AddSingleton<HttpClientHelper>();
        return builder;
    }
}