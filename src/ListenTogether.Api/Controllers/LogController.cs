using Microsoft.AspNetCore.Mvc;
using ListenTogether.Api.Authorization;
using ListenTogether.Api.Interfaces;
using ListenTogether.Model.Api.Request;

namespace ListenTogether.Api.Controllers;
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

    [HttpPost("write")]
    public async Task<IActionResult> Write(LogRequest log)
    {
        //过滤掉2分钟之外的请求
        var time = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToDateTime(log.Timestamp);
        var now = DateTime.Now;
        if (System.Math.Abs(time.Subtract(now).TotalSeconds) > 120)
        {
            return BadRequest("时间戳不合法");
        }

        var response = await _logService.WriteAsync(UserId, log);
        return Ok(response);
    }

    [HttpPost("write-all")]
    public async Task<IActionResult> Write(List<LogRequest> logs)
    {
        var response = await _logService.WriteListAsync(UserId, logs);
        return Ok(response);
    }
}