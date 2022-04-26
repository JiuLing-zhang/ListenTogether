using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Common.Exceptions;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Response;
using System.Text.Json;

namespace MusicPlayerOnline.Data;
public class ApiHttpMessageHandler : DelegatingHandler
{
    public static event EventHandler? TokenUpdated;
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
            throw new AuthorizeException("无效的登录信息");
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
            throw new AuthorizeException("更新认证信息失败");
        }

        DataConfig.UserToken.Token = result.Data.Token;
        DataConfig.UserToken.RefreshToken = result.Data.RefreshToken;
        TokenUpdated?.Invoke(this, EventArgs.Empty);

        request.Headers.Remove("Authorization");
        request.Headers.Add("Authorization", $"Bearer {DataConfig.UserToken.Token}");

        return await base.SendAsync(request, cancellationToken);
    }
}