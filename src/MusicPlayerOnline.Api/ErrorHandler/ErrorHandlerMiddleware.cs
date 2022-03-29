using System.Text.Json;
using MusicPlayerOnline.Api.Models;
using MusicPlayerOnline.Model.Api;

namespace MusicPlayerOnline.Api.ErrorHandler;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            //TODO 记录日志
            var response = context.Response;
            response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new Result(-1, "系统内部异常"));
            await response.WriteAsync(result);
        }
    }
}