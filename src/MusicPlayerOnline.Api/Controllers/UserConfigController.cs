using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

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
            var response = await _userConfigService.ReadAllConfigAsync(user.Id);
            return Ok(response);
        }

        [HttpPost("general")]
        public async Task<IActionResult> WriteGeneralConfig(GeneralSetting model)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var result = await _userConfigService.WriteGeneralConfigAsync(user.Id, model);
            var response = result ? new JsonResultDto(0, "设置成功") : new JsonResultDto(1, "设置失败");
            return Ok(response);
        }

        [HttpPost("play")]
        public async Task<IActionResult> WritePlayConfig(PlaySetting model)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var result = await _userConfigService.WritePlayConfigAsync(user.Id, model);
            var response = result ? new JsonResultDto(0, "设置成功") : new JsonResultDto(1, "设置失败");
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> WriteSearchConfig(SearchSetting model)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var result = await _userConfigService.WriteSearchConfigAsync(user.Id, model);
            var response = result ? new JsonResultDto(0, "设置成功") : new JsonResultDto(1, "设置失败");
            return Ok(response);
        }
    }
}
