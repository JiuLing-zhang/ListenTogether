﻿using System.Text.Json;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Request;
using MusicPlayerOnline.Model.Api.Response;

namespace MusicPlayerOnline.Data.Repositories.Api;
public class UserApiRepository : IUserRepository
{
    public async Task<bool> Register(string username, string password)
    {
        var data = new UserRequest()
        {
            Username = username,
            Password = password
        };
        var sc = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithNoToken.PostAsync(DataConfig.ApiSetting.User.Register, sc);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Result>(json);
        if (result == null)
        {
            return false;
        }
        return true;
    }

    public async Task<User?> Login(string username, string password)
    {
        var data = new UserRequest()
        {
            Username = username,
            Password = password
        };
        var sc = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithNoToken.PostAsync(DataConfig.ApiSetting.User.Login, sc);
        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Result<UserResponse>>(json);
        if (result == null || result.Data == null)
        {
            return default;
        }

        return new User()
        {
            UserName = result.Data.UserName,
            Nickname = result.Data.Nickname,
            Avatar = result.Data.Avatar,
            Token = result.Data.Token,
            RefreshToken = result.Data.RefreshToken,
        };
    }

    public async Task<bool> Logout()
    {
        var response = await DataConfig.HttpClientWithNoToken.PostAsync(DataConfig.ApiSetting.User.Logout, null);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Result>(json);
        if (result == null)
        {
            return false;
        }

        return true;
    }
}