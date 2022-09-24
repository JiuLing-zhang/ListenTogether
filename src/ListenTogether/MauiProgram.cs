using CommunityToolkit.Maui;
using JiuLing.Controls.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.LifecycleEvents;
using NativeMediaMauiLib;
using System.IO;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace ListenTogether
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

#if ANDROID
            using var appIconStream = FileSystem.OpenAppPackageFileAsync("appicon.png").Result;
            using StreamReader reader = new StreamReader(appIconStream);
            var ms = new MemoryStream();
            appIconStream.CopyTo(ms);
            GlobalConfig.AppIcon = ms.ToArray();
#endif

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseJiuLingControls()
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

            return builder.Build();
        }
    }
}