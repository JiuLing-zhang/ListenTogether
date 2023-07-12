namespace ListenTogether.Pages;
public interface IKeyValueStorage
{
    public Task SetAsync(string key, string value);
    public Task SetAsync(string key, int value);
    public Task<string> GetAsync(string key, string defaultValue);
    public Task<int> GetAsync(string key, int defaultValue);
}