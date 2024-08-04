using LogicSimulator.Models;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IOutputMessagesService
{
    void AddErrorMessage(string text);

    void AddWarningMessage(string text);

    void AddInfoMessage(string text);

    void AddDebugMessage(string text);

    void AddErrorMessage(string text, IMessageSource? source);

    void AddWarningMessage(string text, IMessageSource? source);

    void AddInfoMessage(string text, IMessageSource? source);

    void AddDebugMessage(string text, IMessageSource? source);

    void ClearMessages();
}