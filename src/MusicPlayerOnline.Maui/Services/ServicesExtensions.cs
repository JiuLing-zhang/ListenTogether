namespace MusicPlayerOnline.Maui.Services;

internal static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<WifiOptionsService>();
        builder.Services.AddSingleton<PlayerService>();
        return builder;
    }
}