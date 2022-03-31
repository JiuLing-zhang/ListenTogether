namespace MusicPlayerOnline.Maui.ViewModels;

public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<SearchResultPageViewModel>();
        builder.Services.AddTransient<SearchResultViewModel>();
        builder.Services.AddSingleton<ShellViewModel>();
        builder.Services.AddSingleton<SettingPageViewModel>();
        return builder;
    }
}
