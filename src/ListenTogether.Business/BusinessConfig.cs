using ListenTogether.Data.Api;

namespace ListenTogether.Business;

public class BusinessConfig
{
    public static void SetWebApi(string deviceId)
    {
        DataConfig.SetWebApi(deviceId);
    }
}