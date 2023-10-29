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
using ListenTogether.Data.Maui;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using NetMusicLib;

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
                       var id = Win32Interop.GetWindowIdFromWindow(handle);
                       var appWindow = AppWindow.GetFromWindowId(id);
                       switch (appWindow.Presenter)
                       {
                           case OverlappedPresenter overlappedPresenter:
                               overlappedPresenter.SetBorderAndTitleBar(false, false);
                               break;
                       }
                       window.VisibilityChanged += (_, e) =>
                       {
                           if (e.Visible == false)
                           {
                               return;
                           }

                           var mauiWindow = App.Current.Windows.First();
                           var nativeWindow = mauiWindow.Handler.PlatformView;
                           IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                           WindowId windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
                           AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
                           if (((OverlappedPresenter)appWindow.Presenter).State == OverlappedPresenterState.Maximized)
                           {
                               Task.Run(() =>
                               {
                                   //等待窗口展开
                                   Thread.Sleep(200);
                                   MainThread.BeginInvokeOnMainThread(() =>
                                   {
                                       var taskBarHandle = Windows.Win32.PInvoke.FindWindow("Shell_traywnd", "");
                                       Windows.Win32.PInvoke.GetWindowRect(taskBarHandle, out var rct);
                                       var shellHeight = (rct.bottom - rct.top) / mauiWindow.DisplayDensity;
                                       mauiWindow.Height = mauiWindow.Height - shellHeight;
                                   });
                               });
                           }
                       };
                   });
               });
            });
#endif
            DatabaseProvide.Initialize();
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddProvider(new MyLoggerProvider());
            builder.Services.AddSingleton<ILogManage, LogManage>();

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
            builder.Services.AddSingleton<IWindowMoving, WindowMoving>();
            builder.Services.AddSingleton<IWindowTitleBar, WindowTitleBar>();
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
            builder.Services.AddMusicNetPlatform();
            return builder.Build();
        }
    }
}