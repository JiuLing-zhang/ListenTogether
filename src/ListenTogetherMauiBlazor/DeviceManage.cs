using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
public class DeviceManage : IDeviceManage
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