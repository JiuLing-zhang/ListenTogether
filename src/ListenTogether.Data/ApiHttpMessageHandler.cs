using ListenTogether.Model;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Response;
using ListenTogether.Storage;

namespace ListenTogether.Data;
public class ApiHttpMessageHandler : DelegatingHandler
{ 
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", $"Bearer {UserInfoStorage.GetToken()}");
        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
        {
            return response;
        }

        if (UserInfoStorage.GetRefreshToken().IsEmpty())
        {
            UserInfoStorage.Clear();
            throw new Exception("UserToken或RefreshToken不存在。");
        }

        string content = (new { RefreshToken = UserInfoStorage.GetRefreshToken() }).ToJson();
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
            UserInfoStorage.Clear();
            throw new Exception("更新认证信息失败。");
        }

        UserInfoStorage.SetToken(result.Data.Token);
        UserInfoStorage.SetRefreshToken(result.Data.RefreshToken);

        request.Headers.Remove("Authorization");
        request.Headers.Add("Authorization", $"Bearer {result.Data.Token}");

        return await base.SendAsync(request, cancellationToken);
    }
}