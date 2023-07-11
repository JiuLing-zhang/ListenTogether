using JiuLing.CommonLibs.ExtensionMethods;
using ListenTogether.Data;

namespace ListenTogether.Business;

public class BusinessConfig
{
    public static void SetWebApi(string deviceId)
    {
        DataConfig.SetWebApi(deviceId);
    }
}