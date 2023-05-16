using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
public class AppClose : IAppClose
{
    public void Close()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Application.Current.Quit();
        });
    }
}