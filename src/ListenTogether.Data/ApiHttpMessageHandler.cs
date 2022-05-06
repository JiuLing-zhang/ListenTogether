using ListenTogether.Model;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Data;
public class ApiHttpMessageHandler : DelegatingHandler
{
    public static event EventHandler<TokenInfo?>? TokenUpdated;
    public ApiHttpMessageHandler()
    {
        InnerHandler = new HttpClientHandler();
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", $"Bearer {DataConfig.UserToken?.Token}");
        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
        {
            return response;
        }

        if (DataConfig.UserToken == null || DataConfig.UserToken.RefreshToken.IsEmpty())
        {
            TokenUpdated?.Invoke(this, null);
            throw new Exception("UserToken或RefreshToken不存在。");
        }

        string content = (new { DataConfig.UserToken.RefreshToken }).ToJson();
        var sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var refreshTokenRequest = new HttpRequestMessage(HttpMethod.Post, DataConfig.ApiSetting.User.RefreshToken)
        {
            Content = sc
        };

        using var refreshTokenResponse = await base.SendAsync(refreshTokenRequest, cancellationToken);
        var json = await refreshTokenResponse.Content.ReadAsStringAsync(cancellationToken);
        var result = json.ToObject<Result<UserResponse>>();
        if (result == null || result.Code != 0 || result.Data == null)
        {
            TokenUpdated?.Invoke(this, null);
            throw new Exception("更新认证信息失败。");
        }

        var tokenInfo = new TokenInfo()
        {
            Token = result.Data.Token,
            RefreshToken = result.Data.RefreshToken
        };
        TokenUpdated?.Invoke(this, tokenInfo);

        request.Headers.Remove("Authorization");
        request.Headers.Add("Authorization", $"Bearer {tokenInfo.Token}");

        return await base.SendAsync(request, cancellationToken);
    }
}