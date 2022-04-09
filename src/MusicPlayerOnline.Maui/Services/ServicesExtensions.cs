namespace MusicPlayerOnline.Maui.Services;

internal static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {

#if WINDOWS
        builder.Services.AddSingleton<IAudioService, MusicPlayerOnline.Maui.Platforms.Windows.AudioService>();
#elif ANDROID
        builder.Services.AddSingleton<IAudioService, MusicPlayerOnline.Maui.Platforms.Android.AudioService>();
#endif


        builder.Services.AddTransient<PlayerService>();
        builder.Services.AddTransient<WifiOptionsService>();
        builder.Services.AddSingleton<PlayerService>();
        return builder;
    }
}