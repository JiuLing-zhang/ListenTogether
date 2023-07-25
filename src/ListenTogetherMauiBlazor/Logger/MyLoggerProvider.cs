using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ListenTogetherMauiBlazor.Logger;
internal class MyLoggerProvider : ILoggerProvider
{
    private ConcurrentDictionary<string, MyLogger> _loggers = new();
    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, (category) =>
        {
            return new MyLogger(category);
        });
    }

    public void Dispose()
    {

    }
}