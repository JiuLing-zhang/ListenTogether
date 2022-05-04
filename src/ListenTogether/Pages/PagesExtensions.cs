﻿namespace ListenTogether.Pages;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<MyFavoritePage>();
        builder.Services.AddSingleton<PlayingPage>();
        builder.Services.AddSingleton<PlaylistPage>();
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddSingleton<SettingsPage>();

        builder.Services.AddTransient<MyFavoriteDetailPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<RegisterPage>();
        builder.Services.AddSingleton<CacheCleanPage>();
        builder.Services.AddSingleton<LogPage>();
        return builder;
    }
}