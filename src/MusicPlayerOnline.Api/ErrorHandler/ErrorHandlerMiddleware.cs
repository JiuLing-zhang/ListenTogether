using MusicPlayerOnline.Model.Api;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

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

            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            var result = JsonSerializer.Serialize(new Result(-1, "系统内部异常"), jsonOptions);
            await response.WriteAsync(result);
        }
    }
}