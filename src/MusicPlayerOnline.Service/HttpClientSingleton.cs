using System.Net.Http.Headers;
using System.Text.Json;

namespace MusicPlayerOnline.Service;
internal class HttpClientSingleton
{
    private static readonly HttpClient MyHttpClient = new();
    private static readonly object Obj = new();

    private static HttpClientSingleton _instance = null!;
    public static HttpClientSingleton Instance()
    {
        if (_instance == null)
        {
            lock (Obj)
            {
                if (_instance == null)
                {
                    _instance = new HttpClientSingleton();
                }
            }
        }
        return _instance;
    }

    private string _token = "";
    public void SetToken(string token)
    {
        _token = token;

        var authentication = new AuthenticationHeaderValue("Basic", _token);
        MyHttpClient.DefaultRequestHeaders.Authorization = authentication;
    }

    public void ClearToken()
    {
        _token = "";
        MyHttpClient.DefaultRequestHeaders.Authorization = null;
    }
    public async Task<string> GetStringAsync(string url)
    {
        return await MyHttpClient.GetStringAsync(url);
    }

    public async Task<string> PostStringAsync(string url)
    {
        var response = await MyHttpClient.PostAsync(url, null);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PostReadAsStringAsync<T>(string url, T data)
    {
        string content = JsonSerializer.Serialize(data);
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await MyHttpClient.PostAsync(url, sc);
        return await response.Content.ReadAsStringAsync();
    }
}
