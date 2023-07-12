using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor.Storages;
public class KeyValueStorage : IKeyValueStorage
{
    public Task<string> GetAsync(string key, string defaultValue)
    {
        return Task.FromResult(Preferences.Get(key, defaultValue));
    }

    public Task<int> GetAsync(string key, int defaultValue)
    {
        return Task.FromResult(Preferences.Get(key, defaultValue));
    }
    public Task SetAsync(string key, string value)
    {
        Preferences.Set(key, value);
        return Task.CompletedTask;
    }
    public Task SetAsync(string key, int value)
    {
        Preferences.Set(key, value);
        return Task.CompletedTask;
    }
}