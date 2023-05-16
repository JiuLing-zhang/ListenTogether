using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
public class AppVersion : IAppVersion
{
    public Version GetCurrentVersion()
    {
        return AppInfo.Current.Version;
    }

    public string GetCurrentVersionString()
    {
        return GetCurrentVersion().ToString();
    }
}