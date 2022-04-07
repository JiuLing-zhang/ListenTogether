namespace MusicPlayerOnline.Maui.Services;

internal static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {

#if WINDOWS
        builder.Services.TryAddSingleton<IAudioService, MusicPlayerOnline.Maui.Platforms.Windows.AudioService>();
#elif ANDROID
        builder.Services.TryAddSingleton<IAudioService, MusicPlayerOnline.Maui.Platforms.Android.AudioService>();
#endif

        builder.Services.AddTransient<WifiOptionsService>();
        builder.Services.AddSingleton<PlayerService>();
        return builder;
    }
}