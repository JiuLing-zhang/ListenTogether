using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
public class AppClose : IAppClose
{
    public Task CloseAsync()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Application.Current.Quit();
        });
        return Task.CompletedTask;
    }
}