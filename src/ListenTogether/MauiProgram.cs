using CommunityToolkit.Maui;
using ListenTogether.Handlers.GaussianImage;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using NativeMediaMauiLib;

namespace ListenTogether
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
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
            return builder.Build();
        }
    }
}