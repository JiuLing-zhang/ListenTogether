using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.ApiRequest;

namespace MusicPlayerOnline.Api.Controllers;
[Route("api/log")]
[ApiController]
[Authorize]
public class LogController : ApiBaseController
{
    private readonly ILogService _logService;
    public LogController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpPost()]
    public async Task<IActionResult> Write(Log log)
    {
        //过滤掉2分钟之外的请求
        var time = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToDateTime(log.Timestamp);
        var now = DateTime.Now;
        if (System.Math.Abs(time.Subtract(now).TotalSeconds) > 120)
        {
            return BadRequest("时间戳不合法");
        }

        await _logService.WriteAsync(UserId, log);
        return Ok("ok");
    }
}