using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Data;
public class ApiHttpMessageHandler : DelegatingHandler
{
    private readonly ILoginDataStorage _loginDataStorage;
    private readonly IHttpClientFactory _httpClientFactory;
    public ApiHttpMessageHandler(ILoginDataStorage loginDataStorage, IHttpClientFactory httpClientFactory)
    {
        _loginDataStorage = loginDataStorage;
        _httpClientFactory = httpClientFactory;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", $"Bearer {_loginDataStorage.GetToken()}");
        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
        {
            return response;
        }

        if (_loginDataStorage.GetRefreshToken().IsEmpty())
        {
            _loginDataStorage.Clear();
            throw new Exception("UserToken或RefreshToken不存在。");
        }

        string content = (new { RefreshToken = _loginDataStorage.GetRefreshToken() }).ToJson();
        var sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        using var refreshTokenResponse = await _httpClientFactory.CreateClient("WebAPINoToken").PostAsync(DataConfig.ApiSetting.User.RefreshToken, sc);
        var json = await refreshTokenResponse.Content.ReadAsStringAsync(cancellationToken);
        var result = json.ToObject<Result<UserResponse>>();
        if (result == null || result.Code != 0 || result.Data == null)
        {
            _loginDataStorage.Clear();
            throw new Exception("更新认证信息失败。");
        }

        _loginDataStorage.SetToken(result.Data.Token);
        _loginDataStorage.SetRefreshToken(result.Data.RefreshToken);

        request.Headers.Remove("Authorization");
        request.Headers.Add("Authorization", $"Bearer {result.Data.Token}");

        return await base.SendAsync(request, cancellationToken);
    }
}