namespace MusicPlayerOnline.Service.Net
{
    public interface IHttpClientProvider
    {
        public Task<string> GetStringWithNoTokenAsync(string url);
        public Task<string> PostReadAsStringWithNoTokenAsync(string url);
        public Task<string> PostReadAsStringWithNoTokenAsync<T>(string url, T data);

        public Task<string> GetStringWithTokenAsync(string url);
        public Task<string> PostReadAsStringWithTokenAsync(string url);
        public Task<string> PostReadAsStringWithTokenAsync<T>(string url, T data);
    }
}
