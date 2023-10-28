#if WINDOWS
namespace ListenTogetherMauiBlazor;
internal class WindowsTitleBarService
{
    private WindowsTitleBarService()
    {

    }
    public static void SetTitle(string title)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var mauiWindow = App.Current.Windows.First();
            mauiWindow.Title = title;
        });
    }

    public static void Minimize()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var mauiWindow = App.Current.Windows.First();
            var nativeWindow = mauiWindow.Handler.PlatformView;

            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            PInvoke.User32.ShowWindow(windowHandle, PInvoke.User32.WindowShowStyle.SW_MINIMIZE);
        });
    }

    public static void Maximize()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var mauiWindow = App.Current.Windows.First();
            var nativeWindow = mauiWindow.Handler.PlatformView;

            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            PInvoke.User32.ShowWindow(windowHandle, PInvoke.User32.WindowShowStyle.SW_MAXIMIZE);

            var TaskBarHandle = PInvoke.User32.FindWindow("Shell_traywnd", "");
            PInvoke.User32.GetWindowRect(TaskBarHandle, out var rct);

            var shellHeight = rct.bottom - rct.top;
            mauiWindow.Height = mauiWindow.Height - shellHeight;
        });
    }
    public static void ShowNormal()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var mauiWindow = App.Current.Windows.First();
            var nativeWindow = mauiWindow.Handler.PlatformView;
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            PInvoke.User32.ShowWindow(windowHandle, PInvoke.User32.WindowShowStyle.SW_SHOWNORMAL);
        });
    }
    public static void Close()
    {
        App.Current.Quit();
    }
}
#endif