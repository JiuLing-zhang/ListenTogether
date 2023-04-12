using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using ListenTogether.Controls;

namespace ListenTogether.Utils;

internal class ToastService
{
    public static async Task Show(string message, ToastDuration duration = ToastDuration.Short, double textSize = 14.0)
    {
        Page page = Application.Current?.MainPage;
        //TODO Windows 下的弹窗目前会造成死锁，因此先屏蔽掉
        if (page != null && !Config.Desktop)
        {
            try
            {
                await page.ShowPopupAsync(new MessageDialog(message));
            }
            catch (Exception)
            {
                await ShowUseToast(message, duration, textSize);
            }
        }
        else
        {
            await ShowUseToast(message, duration, textSize);
        }
    }

    private static async Task ShowUseToast(string message, ToastDuration duration = ToastDuration.Short, double textSize = 14.0)
    {
        var windowsToast = Toast.Make(message, duration, textSize);
        await windowsToast.Show();
    }
}