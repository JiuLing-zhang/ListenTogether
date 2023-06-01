using ListenTogether.EasyLog;
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

    public string GetDeviceId()
    {
#if WINDOWS
        try
        {
            var searcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
            var collection = searcher.Get();

            foreach (System.Management.ManagementObject obj in collection)
            {
                return obj["UUID"].ToString().ToLower();
            }
            Logger.Error("设备ID获取失败", new Exception("未能获取到设备信息"));
            return "";
        }
        catch (System.Management.ManagementException ex)
        {
            Logger.Error("设备ID获取失败", ex);
            return "";
        }
#elif ANDROID
        try
        {
            var context = Android.App.Application.Context;
            return Android.Provider.Settings.Secure.GetString(context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
        }
        catch (Exception ex)
        {
            Logger.Error("设备ID获取失败", ex);
            return "";
        }
#else
        return "";
#endif
    }
}