using ListenTogether.EasyLog;
using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
public class DeviceManage : IDeviceManage
{
    public Task ScreenOnAsync()
    {
        if (!Config.Desktop)
        {
            if (DeviceDisplay.Current.KeepScreenOn == false)
            {
                DeviceDisplay.Current.KeepScreenOn = true;
            }
        }

        return Task.CompletedTask;
    }

    public Task ScreenOffAsync()
    {
        if (!Config.Desktop)
        {
            if (DeviceDisplay.Current.KeepScreenOn == true)
            {
                DeviceDisplay.Current.KeepScreenOn = false;
            }
        }
        return Task.CompletedTask;
    }

    public Task<string> GetDeviceIdAsync()
    {
#if WINDOWS
        try
        {
            var searcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
            var collection = searcher.Get();

            foreach (System.Management.ManagementObject obj in collection)
            {
                return Task.FromResult(obj["UUID"].ToString().ToLower());
            }
            Logger.Error("设备ID获取失败", new Exception("未能获取到设备信息"));
            return Task.FromResult("");
        }
        catch (System.Management.ManagementException ex)
        {
            Logger.Error("设备ID获取失败", ex);
            return Task.FromResult("");
        }
#elif ANDROID
        try
        {
            var context = Android.App.Application.Context;
            var deviceId = Android.Provider.Settings.Secure.GetString(context.ContentResolver,
                Android.Provider.Settings.Secure.AndroidId);
            return Task.FromResult(deviceId);
        }
        catch (Exception ex)
        {
            Logger.Error("设备ID获取失败", ex);
            return Task.FromResult("");
        }
#else
        return Task.FromResult("");
#endif
    }
}