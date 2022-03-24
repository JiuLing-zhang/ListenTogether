using MusicPlayerOnline.EasyLog;
using MusicPlayerOnline.Model;
using System.Text.Json;

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
            var response = context.Response;
            response.ContentType = "application/json";
            Logger.Error("API服务内部错误。", ex);
            var result = JsonSerializer.Serialize(new Result(-1, "系统内部异常"));
            await response.WriteAsync(result);
        }
    }
}