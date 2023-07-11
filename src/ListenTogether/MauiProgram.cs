using CommunityToolkit.Maui;
using ListenTogether.Data.Api;
using ListenTogether.Handlers.GaussianImage;
using ListenTogether.Storages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using NativeMediaMauiLib;

namespace ListenTogether
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            using var stream = FileSystem.OpenAppPackageFileAsync("NetConfig.json").Result;
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var netConfig = json.ToObject<NetConfig>();

            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseNativeMedia()
                .UseBusiness()
                .ConfigureServices()
                .ConfigurePages()
                .ConfigureViewModels()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.ConfigureMauiHandlers(handler =>
            {
                handler.AddHandler(typeof(GaussianImage), typeof(GaussianImageHandler));
            });
            builder.ConfigureLifecycleEvents(events =>
            {
#if ANDROID
                events.AddAndroid(android =>
                {
                    //android
                    //.OnActivityResult((activity, requestCode, resultCode, data) => Logger.Info("OnActivityResult"))
                    //.OnApplicationCreate(x => Logger.Info("OnApplicationCreate"))
                    //.OnCreate((activity, bundle) => Logger.Info("OnCreate"))
                    //.OnDestroy(x => Logger.Info("OnDestroy"))
                    //.OnPause(x => Logger.Info("OnPause"))
                    //.OnPostResume(x => Logger.Info("OnPostResume"))
                    //.OnRestart(x => Logger.Info("OnRestart"))
                    //.OnRestoreInstanceState((activity, bundle) => Logger.Info("OnRestoreInstanceState"))
                    //.OnResume(x => Logger.Info("OnResume"))
                    //.OnStart(x => Logger.Info("OnStart"))
                    //.OnStop(x => Logger.Info("OnStop"));

                });
#endif
            });
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ISearchHistoryStorage, SearchHistoryStorage>();
            builder.Services.AddSingleton<ILoginDataStorage, LoginDataStorage>();
            builder.Services.AddSingleton<IMusicCacheStorage, MusicCacheStorage>();
            builder.Services.AddSingleton<ApiHttpMessageHandler>();
            builder.Services.AddHttpClient("WebAPI", httpClient =>
            {
                httpClient.BaseAddress = new Uri(netConfig.ApiDomain);
                httpClient.Timeout = TimeSpan.FromSeconds(15);
            }).AddHttpMessageHandler<ApiHttpMessageHandler>();
            builder.Services.AddHttpClient("WebAPINoToken", httpClient =>
            {
                httpClient.BaseAddress = new Uri(netConfig.ApiDomain);
                httpClient.Timeout = TimeSpan.FromSeconds(15);
            });
            return builder.Build();
        }
    }
}