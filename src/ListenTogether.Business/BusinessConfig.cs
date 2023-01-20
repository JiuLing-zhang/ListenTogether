using JiuLing.CommonLibs.ExtensionMethods;
using ListenTogether.Data;

namespace ListenTogether.Business;

public class BusinessConfig
{
    public static void SetDataBaseConnection(string path)
    {
        if (path.IsEmpty())
        {
            throw new ArgumentException("本地数据库配置参数错误");
        }
        DataConfig.SetDataBaseConnection(path);
    }
    public static void SetWebApi(string apiBaseUrl, string deviceId)
    {
        if (apiBaseUrl.IsEmpty() || deviceId.IsEmpty())
        {
            throw new ArgumentException("Web API配置参数错误");
        }
        DataConfig.SetWebApi(apiBaseUrl, deviceId);
    }
}