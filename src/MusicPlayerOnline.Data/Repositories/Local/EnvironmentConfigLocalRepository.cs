using System.Text.Json;
using MusicPlayerOnline.Data.Entities;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Data.Repositories.Local;

public class EnvironmentConfigLocalRepository : IEnvironmentConfigRepository
{
    public async Task<EnvironmentSetting> ReadAllSettingsAsync()
    {
        var environmentConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (environmentConfig == null)
        {
            environmentConfig = await InitializationEnvironmentSetting();
        }

        var result = new EnvironmentSetting();
        var playerSetting = JsonSerializer.Deserialize<PlayerSetting>(environmentConfig.PlayerSettingJson) ?? throw new Exception("配置信息不存在：PlayerSettingJson");
        result.Player = new PlayerSetting()
        {
            Voice = playerSetting.Voice,
            IsSoundOff = playerSetting.IsSoundOff,
            PlayMode = playerSetting.PlayMode
        };

        return result;
    }

    private async Task<EnvironmentConfigEntity> InitializationEnvironmentSetting()
    {
        var environmentConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (environmentConfig != null)
        {
            return environmentConfig;
        }

        environmentConfig = new EnvironmentConfigEntity()
        {
            PlayerSettingJson = JsonSerializer.Serialize(new PlayerSetting()
            {
                Voice = 50,
                IsSoundOff = false,
                PlayMode = PlayModeEnum.RepeatList
            })
        };

        var count = await DatabaseProvide.DatabaseAsync.InsertAsync(environmentConfig);
        if (count == 0)
        {
            throw new Exception("初始化环境配置失败");
        }
        return environmentConfig;
    }

    public async Task WritePlayerSettingAsync(PlayerSetting playerSetting)
    {
        var environmentConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (environmentConfig == null)
        {
            throw new Exception("环境配置信息不存在，播放设置保存失败");
        }

        environmentConfig.PlayerSettingJson = JsonSerializer.Serialize(playerSetting);
        await DatabaseProvide.DatabaseAsync.UpdateAsync(environmentConfig);
    }
}