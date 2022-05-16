using CommunityToolkit.Maui;
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

                        const int width = 1100;
                        const int height = 650;
                        Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(win32WindowsId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);

                        appWindow.Changed+=(aw,e)=>{
                            if(!e.DidSizeChange)
                            {
                                return;
                            }
                            if (aw.Size.Width<width)
	                        {
                                appWindow.Resize(new SizeInt32(width, aw.Size.Height));
	                        }
                            if (aw.Size.Height<height)
	                        {
                                appWindow.Resize(new SizeInt32(aw.Size.Width, height));
	                        }
                        };
                        appWindow.MoveAndResize(new RectInt32(displayArea.WorkArea.Width / 2 - width / 2, displayArea.WorkArea.Height / 2 - height / 2, width, height));
                    });
                });
            });
#endif
            return builder.Build();
        }
    }
}