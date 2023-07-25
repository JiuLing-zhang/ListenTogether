using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenTogetherMauiBlazor.Logger;
internal class MyLogger : ILogger
{
    private string _category;
    public MyLogger(string category)
    {
        _category = category;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        string exMessage = "";
        if (exception != null)
        {
            exMessage = $"{Environment.NewLine}{exception.Message}.{Environment.NewLine}{exception.StackTrace}";
            if (exception.InnerException != null)
            {
                exMessage += $"{Environment.NewLine}Inner Error：{exception.InnerException.Message}.{Environment.NewLine}{exception.InnerException.StackTrace}";
            }
        }

        string message = $"{_category}{Environment.NewLine}{state}{exMessage}";
        try
        {
            var logEntity = new LogEntity()
            {
                CreateTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
                LogType = (int)logLevel,
                Message = message
            };
            LogDatabaseProvide.Database.Insert(logEntity);
        }
        catch (Exception)
        {
        }
    }
}