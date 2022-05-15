using JiuLing.CommonLibs.Net;

namespace ListenTogether.Services;

internal static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {

#if WINDOWS
        builder.Services.AddSingleton<IAudioService, ListenTogether.Platforms.Windows.AudioService>();
        builder.Services.AddSingleton<IBlurredImageService, ListenTogether.Platforms.Windows.BlurredImageService>();
      
#elif ANDROID
        builder.Services.AddSingleton<IAudioService, ListenTogether.Platforms.Android.AudioService>();
#endif

        builder.Services.AddTransient<WifiOptionsService>();
        builder.Services.AddSingleton<PlayerService>();
        builder.Services.AddSingleton<HttpClientHelper>();
        return builder;
    }
}