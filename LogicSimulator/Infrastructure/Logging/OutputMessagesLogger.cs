using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Common;
using Microsoft.Extensions.Logging;

namespace LogicSimulator.Infrastructure.Logging;

public class OutputMessagesLogger : ILogger
{
    private readonly IOutputMessagesService _outputMessagesService;
    private readonly string _categoryName;

    public OutputMessagesLogger(IOutputMessagesService outputMessagesService, string categoryName)
    {
        _outputMessagesService = outputMessagesService;
        _categoryName = categoryName;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var msg = formatter(state, exception);
        var source = new CommonMessageSource(_categoryName);

        switch (logLevel)
        {
            case LogLevel.None:
            case LogLevel.Trace:
            case LogLevel.Debug:
                _outputMessagesService.AddDebugMessage(msg, source);
                break;

            case LogLevel.Information:
                _outputMessagesService.AddInfoMessage(msg, source);
                break;

            case LogLevel.Warning:
                _outputMessagesService.AddWarningMessage(msg, source);
                break;

            case LogLevel.Error:
            case LogLevel.Critical:
                _outputMessagesService.AddErrorMessage(msg, source);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
        }
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}