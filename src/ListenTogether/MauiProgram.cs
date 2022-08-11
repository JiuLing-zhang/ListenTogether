using CommunityToolkit.Maui;
using JiuLing.Controls.Maui;
using ListenTogether.Services.MusicSwitchServer;
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
            using var stream = FileSystem.OpenAppPackageFileAsync("Settings.json").Result;
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
            builder.Configuration.AddConfiguration(config);

            builder.Services.AddSingleton<IMusicSwitchServer, MusicSwitchRepeatListServer>();
            builder.Services.AddSingleton<IMusicSwitchServer, MusicSwitchRepeatOneServer>();
            builder.Services.AddSingleton<IMusicSwitchServer, MusicSwitchShuffleServer>();
            builder.Services.AddSingleton<IMusicSwitchServerFactory, MusicSwitchServerFactory>();

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

#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(wndLifeCycleBuilder =>
                {
                    wndLifeCycleBuilder.OnWindowCreated(window =>
                    {                        
                        IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                        AppWindow appWindow = AppWindow.GetFromWindowId(win32WindowsId);

                        var _presenter = appWindow.Presenter as OverlappedPresenter;
                        _presenter.IsResizable=false;

                        Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(win32WindowsId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                        
                        //简单适配不同分辨率
                        int minWidth;
                        int minHeight;
                        if (displayArea.WorkArea.Width>2000)
                        {
                            minWidth=1650;
                            minHeight=975;
                        }else if (displayArea.WorkArea.Width>1000)
	                    {
                            minWidth=1100;
                            minHeight=650;
	                    }else
                        {
                            minWidth=880;
                            minHeight=520;
                        }

                        appWindow.Changed+=(aw,e)=>{
                            if(!e.DidSizeChange)
                            {
                                return;
                            }
                            if (_presenter.State==OverlappedPresenterState.Maximized)
	                        {
                                _presenter.Restore();
	                        }                            
                            if (aw.Size.Width<minWidth)
	                        {
                                appWindow.Resize(new SizeInt32(minWidth, aw.Size.Height));
	                        }
                            if (aw.Size.Height<minHeight)
	                        {
                                appWindow.Resize(new SizeInt32(aw.Size.Width, minHeight));
	                        }
                        };
                        appWindow.MoveAndResize(new RectInt32(displayArea.WorkArea.Width / 2 - minWidth / 2, displayArea.WorkArea.Height / 2 - minHeight / 2, minWidth, minHeight));
                    });
                });
            });
#endif
            return builder.Build();
        }
    }
}