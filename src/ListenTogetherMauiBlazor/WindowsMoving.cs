#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System.Runtime.CompilerServices;

namespace ListenTogetherMauiBlazor;
internal class WindowsMoving
{
    static AppWindow appWindow;

    private static bool _isMoving = false;
    public static void MouseDown()
    {
        _isMoving = true;

        if (appWindow == null)
        {
            var mauiWindow = App.Current.Windows.First();
            var nativeWindow = mauiWindow.Handler.PlatformView;
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);

            WindowId WindowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            appWindow = AppWindow.GetFromWindowId(WindowId);
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            PInvoke.User32.GetCursorPos(out var point);
            appWindow.Move(new Windows.Graphics.PointInt32(point.x, point.y));

        });
    }


    public static void MouseUp()
    {
        _isMoving = false;
    }
}
#endif