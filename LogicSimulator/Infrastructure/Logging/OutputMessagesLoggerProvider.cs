using LogicSimulator.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace LogicSimulator.Infrastructure.Logging;

public class OutputMessagesLoggerProvider : ILoggerProvider
{
    private readonly IOutputMessagesService _outputMessagesService;

    public OutputMessagesLoggerProvider(IOutputMessagesService outputMessagesService) => _outputMessagesService = outputMessagesService;

    public ILogger CreateLogger(string categoryName) => new OutputMessagesLogger(_outputMessagesService, categoryName);

    public void Dispose() { }
}