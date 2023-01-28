using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using ListenTogether.Controls;

namespace ListenTogether.Utils;

internal class ToastService
{
    public static async Task Show(string message, ToastDuration duration = ToastDuration.Short, double textSize = 14.0)
    {
#if WINDOWS
        Page page = Application.Current?.MainPage;
        if (page != null)
        {
            try
            {
                await page.ShowPopupAsync(new MessageDialog(message));
            }
            catch (Exception)
            {

            }
        }
        else
        {
            var windowsToast = Toast.Make(message, duration, textSize);
            await windowsToast.Show();
        }
#else
        var toast = Toast.Make(message, duration, textSize);
        await toast.Show();
#endif
    }
}