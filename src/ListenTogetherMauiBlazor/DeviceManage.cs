using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
public class DeviceScreen : IDeviceScreen
{
    public void ScreenOn()
    {
        if (!Config.Desktop)
        {
            if (DeviceDisplay.Current.KeepScreenOn == false)
            {
                DeviceDisplay.Current.KeepScreenOn = true;
            }
        }
    }

    public void ScreenOff()
    {
        if (!Config.Desktop)
        {
            if (DeviceDisplay.Current.KeepScreenOn == true)
            {
                DeviceDisplay.Current.KeepScreenOn = false;
            }
        }
    }
}