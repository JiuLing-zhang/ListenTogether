using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
public class AppVersion : IAppVersion
{
    public Task<Version> GetCurrentVersionAsync()
    {
        return Task.FromResult(AppInfo.Current.Version);
    }

    public async Task<string> GetCurrentVersionStringAsync()
    {
        return (await GetCurrentVersionAsync()).ToString();
    }
}