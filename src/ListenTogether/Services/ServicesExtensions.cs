namespace ListenTogether.Services;

internal static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {

#if WINDOWS
        builder.Services.AddSingleton<IAudioService, ListenTogether.Platforms.Windows.AudioService>();
#elif ANDROID
        builder.Services.AddSingleton<IAudioService, ListenTogether.Platforms.Android.AudioService>();
#endif

        builder.Services.AddTransient<WifiOptionsService>();
        builder.Services.AddSingleton<PlayerService>();
        return builder;
    }
}