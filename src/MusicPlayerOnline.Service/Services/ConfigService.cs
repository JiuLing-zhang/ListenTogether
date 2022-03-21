using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service.Services
{
    public class ConfigService
    {
        public Task<UserSettingDto> ReadAllConfigAsync()
        {
            throw new NotImplementedException();
        }

        public Task WriteGeneralConfigAsync(GeneralSetting generalSetting)
        {
            throw new NotImplementedException();
        }

        public Task WritePlatformConfigAsync(SearchSetting searchSetting)
        {
            throw new NotImplementedException();
        }

        public Task WritePlayConfigAsync(PlaySetting playSetting)
        {
            throw new NotImplementedException();
        }
    }
}
