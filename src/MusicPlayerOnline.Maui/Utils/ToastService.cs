using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace MusicPlayerOnline.Maui.Utils;

internal class ToastService
{
    public static async Task Show(string message, ToastDuration duration = ToastDuration.Short, double textSize = 14.0)
    {
        var toast = Toast.Make(message, duration, textSize);
        await toast.Show();
    }
}