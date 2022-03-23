using System.Text.Json;

namespace MusicPlayerOnline.Service.Net;
internal class HttpClientProvider : IHttpClientProvider
{
    private static ApiHttpMessageHandler? _apiHttpMessageHandler;
    public HttpClientProvider()
    {

    }
    public HttpClientProvider(ApiHttpMessageHandler apiHttpMessageHandler)
    {
        _apiHttpMessageHandler = apiHttpMessageHandler;
    }

    private static readonly Lazy<HttpClient> ClientWithNoToken = new(() => new HttpClient());
    private static readonly Lazy<HttpClient> ClientWithToken = new(() =>
        {
            if (_apiHttpMessageHandler == null)
            {
                throw new Exception("初始化HTTP组件失败");
            }
            return new HttpClient(_apiHttpMessageHandler);
        }
    );
    
    public async Task<string> GetStringWithNoTokenAsync(string url)
    {
        return await ClientWithNoToken.Value.GetStringAsync(url);
    }

    public async Task<string> PostReadAsStringWithNoTokenAsync(string url)
    {
        var response = await ClientWithNoToken.Value.PostAsync(url, null);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PostReadAsStringWithNoTokenAsync<T>(string url, T data)
    {
        string content = JsonSerializer.Serialize(data);
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await ClientWithNoToken.Value.PostAsync(url, sc);
        return await response.Content.ReadAsStringAsync();
    }


    public async Task<string> GetStringWithTokenAsync(string url)
    {
        return await ClientWithToken.Value.GetStringAsync(url);
    }

    public async Task<string> PostReadAsStringWithTokenAsync(string url)
    {
        var response = await ClientWithToken.Value.PostAsync(url, null);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PostReadAsStringWithTokenAsync<T>(string url, T data)
    {
        string content = JsonSerializer.Serialize(data);
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await ClientWithToken.Value.PostAsync(url, sc);
        return await response.Content.ReadAsStringAsync();
    }
}