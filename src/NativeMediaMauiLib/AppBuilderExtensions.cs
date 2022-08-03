
namespace NativeMediaMauiLib;
public static class AppBuilderExtensions
{
    public static MauiAppBuilder UseNativeMedia(this MauiAppBuilder builder)
    {
#if WINDOWS
        builder.Services.AddSingleton<INativeAudioService, Platforms.Windows.NativeAudioService>();
#elif ANDROID
        builder.Services.AddSingleton<INativeAudioService, Platforms.Android.NativeAudioService>();
#elif MACCATALYST
        builder.Services.AddSingleton<INativeAudioService, Platforms.MacCatalyst.NativeAudioService>();
#elif IOS
        builder.Services.AddSingleton<INativeAudioService, Platforms.iOS.NativeAudioService>();
#endif
        return builder;
    }
}