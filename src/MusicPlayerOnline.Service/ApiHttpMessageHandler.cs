using System.Text.Json;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service;
internal class ApiHttpMessageHandler : DelegatingHandler
{
    private readonly ILocalTokenService _localTokenService;
    private readonly TokenInfo _tokenInfo;
    public ApiHttpMessageHandler(ILocalTokenService localTokenService)
    {
        _localTokenService = localTokenService;
        _tokenInfo = _localTokenService.Read().Result;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", $"Bearer {_tokenInfo.Token}");
        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
        {
            return response;
        }

        string content = JsonSerializer.Serialize(new { _tokenInfo.RefreshToken });
        var sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var refreshTokenRequest = new HttpRequestMessage(HttpMethod.Post, GlobalConfig.ApiSetting.User.RefreshToken)
        {
            Content = sc
        };

        using var refreshTokenResponse = await base.SendAsync(refreshTokenRequest, cancellationToken);
        var json = await refreshTokenResponse.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<Result<UserDto>>(json);
        if (result == null || result.Code != 0 || result.Data == null)
        {
            //刷新失败时，返回原有的 response
            return response;
        }

        _tokenInfo.Token = result.Data.Token;
        _tokenInfo.RefreshToken = result.Data.RefreshToken;
        if (await _localTokenService.Write(_tokenInfo) == false)
        {
            //新的 token 保存失败时，也返回原有的 response
            return response;
        }

        request.Headers.Remove("Authorization");
        request.Headers.Add("Authorization", $"Bearer {_tokenInfo.Token}");

        return await base.SendAsync(request, cancellationToken);
    }
}