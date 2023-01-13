namespace ListenTogether.Pages;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<MyFavoritePage>();
        builder.Services.AddSingleton<PlaylistPage>();
        builder.Services.AddTransient<SearchResultPage>();
        builder.Services.AddSingleton<SettingsPage>();

        builder.Services.AddTransient<PlayingPage>();
        builder.Services.AddTransient<MyFavoriteDetailPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<RegisterPage>();
        builder.Services.AddSingleton<CacheCleanPage>();
        builder.Services.AddSingleton<LogPage>();
        builder.Services.AddSingleton<SearchPage>();

        builder.Services.AddSingleton<MiGuPage>();
        builder.Services.AddSingleton<ChooseTagPage>();
        return builder;
    }
}
