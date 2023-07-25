using JiuLing.CommonLibs.ExtensionMethods;
using ListenTogether.Pages;
using ListenTogetherMauiBlazor.Storages;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Microsoft.Maui.LifecycleEvents;
using NativeMediaMauiLib;
using CommunityToolkit.Maui;
using ListenTogether.Data.Api;
using ListenTogetherMauiBlazor.Logger;
using ListenTogether.Service.Interface;

namespace ListenTogetherMauiBlazor
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(windowsLifecycleBuilder =>
               {
                   windowsLifecycleBuilder.OnWindowCreated(window =>
                   {
                       window.ExtendsContentIntoTitleBar = false;
                       var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                       var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                       var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                       switch (appWindow.Presenter)
                       {
                           case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                               overlappedPresenter.SetBorderAndTitleBar(false, false);
                               break;
                       }
                   });
               });
            });
#endif

            //builder.Services.AddBlazorWebViewDeveloperTools();
            //builder.Logging.AddProvider(new MyLoggerProvider());
            //builder.Services.AddSingleton<ILogManage, LogManage>();

            using var stream = FileSystem.OpenAppPackageFileAsync("NetConfig.json").Result;
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            builder.Services.AddSingleton<NetConfig>(json.ToObject<NetConfig>());

            builder.Services.AddSingleton<CustomTheme>();
            builder.Services.AddSingleton<AutoCloseJob>();
            builder.Services.AddSingleton<IDeviceManage, DeviceManage>();
            builder.Services.AddSingleton<IMusicShare, MusicShare>();
            builder.Services.AddSingleton<INativeTheme, NativeTheme>();
            builder.Services.AddSingleton<IAppClose, AppClose>();
            builder.Services.AddSingleton<IAutoUpgrade, AutoUpgrade>();
            builder.Services.AddSingleton<IAppVersion, AppVersion>();
            builder.Services.AddSingleton<DesktopMoving>();
            builder.Services.AddSingleton<DesktopNotification>();
            builder.Services.AddSingleton<IPlayHistoryStorage, PlayHistoryStorage>();
            builder.Services.AddSingleton<ISearchHistoryStorage, SearchHistoryStorage>();
            builder.Services.AddSingleton<ILoginDataStorage, LoginDataStorage>();
            builder.Services.AddSingleton<IKeyValueStorage, KeyValueStorage>();
            builder.Services.AddSingleton<IMusicCacheStorage, MusicCacheStorage>();
            builder.Services.AddSingleton<ApiHttpMessageHandler>();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient("WebAPI", (sp, httpClient) =>
            {
                httpClient.BaseAddress = new Uri(sp.GetService<NetConfig>().ApiDomain);
                httpClient.Timeout = TimeSpan.FromSeconds(15);
            }).AddHttpMessageHandler<ApiHttpMessageHandler>();
            builder.Services.AddHttpClient("WebAPINoToken", (sp, httpClient) =>
            {
                httpClient.BaseAddress = new Uri(sp.GetService<NetConfig>().ApiDomain);
                httpClient.Timeout = TimeSpan.FromSeconds(15);
            });
            builder.Services.AddHttpClient("", httpClient =>
            {
                httpClient.Timeout = TimeSpan.FromSeconds(15);
            });

            builder.Services.AddBusiness();
            builder.Services.AddMudServices();
            return builder.Build();
        }
    }
}