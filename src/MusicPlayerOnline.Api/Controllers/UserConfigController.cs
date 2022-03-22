using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.ApiRequest;

namespace MusicPlayerOnline.Api.Controllers
{
    [Route("api/user-config")]
    [ApiController]
    [Authorize]
    public class UserConfigController : ApiBaseController
    {
        private readonly IUserConfigService _userConfigService;
        public UserConfigController(IUserConfigService userConfigService)
        {
            _userConfigService = userConfigService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var response = await _userConfigService.ReadAllSettingAsync(UserId);
            return Ok(response);
        }

        [HttpPost("general")]
        public async Task<IActionResult> WriteGeneralConfig(GeneralSetting model)
        {
            var response = await _userConfigService.WriteGeneralSettingAsync(UserId, model);
            return Ok(response);
        }

        [HttpPost("play")]
        public async Task<IActionResult> WritePlayConfig(PlaySetting model)
        {
            var response = await _userConfigService.WritePlaySettingAsync(UserId, model);
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> WriteSearchConfig(SearchSetting model)
        {
            var response = await _userConfigService.WriteSearchSettingAsync(UserId, model);
            return Ok(response);
        }
    }
}
