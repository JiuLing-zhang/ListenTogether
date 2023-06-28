namespace ListenTogether.Pages;
public interface IKeyValueStorage
{
    public Task Set(string key, string value);
    public Task Set(string key, int value);
    public Task<string> Get(string key, string defaultValue);
    public Task<int> Get(string key, int defaultValue);
}