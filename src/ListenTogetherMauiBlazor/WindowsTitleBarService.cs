#if WINDOWS
namespace ListenTogetherMauiBlazor;
internal class WindowsTitleBarService
{
    private WindowsTitleBarService()
    {

    }

    public static void Minimize()
    {
        var mauiWindow = App.Current.Windows.First();
        var nativeWindow = mauiWindow.Handler.PlatformView;

        IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
        PInvoke.User32.ShowWindow(windowHandle, PInvoke.User32.WindowShowStyle.SW_MINIMIZE);
    }

    public static void Maximize()
    {
        var mauiWindow = App.Current.Windows.First();
        var nativeWindow = mauiWindow.Handler.PlatformView;

        IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
        PInvoke.User32.ShowWindow(windowHandle, PInvoke.User32.WindowShowStyle.SW_MAXIMIZE);

        var TaskBarHandle = PInvoke.User32.FindWindow("Shell_traywnd", "");
        PInvoke.User32.GetWindowRect(TaskBarHandle, out var rct);
   
        mauiWindow.X = 0;
        mauiWindow.Y = 0;
        mauiWindow.Height = rct.top;
    }
    public static void ShowNormal()
    {
        var mauiWindow = App.Current.Windows.First();
        var nativeWindow = mauiWindow.Handler.PlatformView;
        IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
        PInvoke.User32.ShowWindow(windowHandle, PInvoke.User32.WindowShowStyle.SW_SHOWNORMAL);
    }
    public static void Close()
    {
        App.Current.Quit();
    }
}
#endif