using CommunityToolkit.Maui;
using ListenTogether.Handlers.GaussianBlurHandler;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.LifecycleEvents;

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

            using var stream = FileSystem.OpenAppPackageFileAsync("Settings.json").Result;
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
            builder.Configuration.AddConfiguration(config);

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBusiness()
                .ConfigureServices()
                .ConfigurePages()
                .ConfigureViewModels()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(handler =>
                {
                    handler.AddHandler(typeof(GaussianImage), typeof(GaussianImageHandler));
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