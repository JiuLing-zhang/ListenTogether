using JiuLing.CommonLibs.Net;

namespace ListenTogether.Services;

internal static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<WifiOptionsService>();
        builder.Services.AddSingleton<PlayerService>();
        builder.Services.AddSingleton<HttpClientHelper>();
        return builder;
    }
}