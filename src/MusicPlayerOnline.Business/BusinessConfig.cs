﻿using MusicPlayerOnline.Data;

namespace MusicPlayerOnline.Business;

public class BusinessConfig
{
    public static void SetWebApi(string localDbPath, string apiBaseUrl, string deviceId)
    {
        DataConfig.SetDataConnection(localDbPath, apiBaseUrl, deviceId);
    }

    /// <summary>
    /// 是否使用 API 接口
    /// </summary>
    public static bool IsUseApiInterface { get; set; }
}