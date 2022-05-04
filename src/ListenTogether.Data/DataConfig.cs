﻿using ListenTogether.Model;

namespace ListenTogether.Data;

public class DataConfig
{
    internal static readonly HttpClient HttpClientWithNoToken = new();
    internal static readonly HttpClient HttpClientWithToken = new(new ApiHttpMessageHandler());

    /// <summary>
    /// API 的一些配置信息
    /// </summary>
    internal static ApiSettings ApiSetting { get; set; } = null!;

    /// <summary>
    /// 用于认证的Token 信息
    /// </summary>
    public static TokenInfo? UserToken { get; set; }
    /// <summary>
    /// Token 已更新
    /// </summary>
    public static event EventHandler? TokenUpdated;

    public static void SetDataConnection(string localDbPath, string apiBaseUrl, string deviceId)
    {
        ApiHttpMessageHandler.TokenUpdated += TokenUpdated;

        DatabaseProvide.SetConnection(localDbPath);
        ApiSetting = new ApiSettings(apiBaseUrl, deviceId);
    }
}