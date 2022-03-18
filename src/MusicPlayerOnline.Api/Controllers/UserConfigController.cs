using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.ApiRequest;

namespace MusicPlayerOnline.Api.Controllers
{
    [Route("api/user-config")]
    [ApiController]
    [Authorize]
    public class UserConfigController : ControllerBase
    {
        private readonly IUserConfigService _userConfigService;
        public UserConfigController(IUserConfigService userConfigService)
        {
            _userConfigService = userConfigService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _userConfigService.ReadAllSettingAsync(user.Id);
            return Ok(response);
        }

        [HttpPost("general")]
        public async Task<IActionResult> WriteGeneralConfig(GeneralSetting model)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _userConfigService.WriteGeneralSettingAsync(user.Id, model);
            return Ok(response);
        }

        [HttpPost("play")]
        public async Task<IActionResult> WritePlayConfig(PlaySetting model)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _userConfigService.WritePlaySettingAsync(user.Id, model);
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> WriteSearchConfig(SearchSetting model)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _userConfigService.WriteSearchSettingAsync(user.Id, model);
            return Ok(response);
        }
    }
}
