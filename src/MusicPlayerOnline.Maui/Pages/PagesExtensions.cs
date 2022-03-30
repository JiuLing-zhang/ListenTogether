namespace MusicPlayerOnline.Maui.Pages;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MyFavoritePage>();
        builder.Services.AddSingleton<PlayingPage>();
        builder.Services.AddSingleton<PlaylistPage>();
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddSingleton<SettingsPage>();

        //// pages that are navigated to
        //builder.Services.AddTransient<CategoriesPage>();
        //builder.Services.AddTransient<CategoryPage>();
        //builder.Services.AddTransient<EpisodeDetailPage>();
        //builder.Services.AddTransient<ShowDetailPage>();
        //builder.Services.AddTransient<SubscriptionsPage>();        

        return builder;
    }
}
