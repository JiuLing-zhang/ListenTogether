using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor.Storages;
public class KeyValueStorage : IKeyValueStorage
{
    public Task<string> Get(string key, string defaultValue)
    {
        return Task.FromResult(Preferences.Get(key, defaultValue));
    }

    public Task<int> Get(string key, int defaultValue)
    {
        return Task.FromResult(Preferences.Get(key, defaultValue));
    }
    public Task Set(string key, string value)
    {
        Preferences.Set(key, value);
        return Task.CompletedTask;
    }
    public Task Set(string key, int value)
    {
        Preferences.Set(key, value);
        return Task.CompletedTask;
    }
}