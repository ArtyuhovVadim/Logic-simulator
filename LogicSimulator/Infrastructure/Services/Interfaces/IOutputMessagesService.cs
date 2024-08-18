using LogicSimulator.Models.Common;
using LogicSimulator.Models.MessageSources;
using LogicSimulator.ViewModels.Common;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IOutputMessagesService
{
    void AddMessage(OutputMessageViewModel message);

    void AddMessage(string text, MessageType messageType);

    void AddErrorMessage(string text);

    void AddWarningMessage(string text);

    void AddInfoMessage(string text);

    void AddDebugMessage(string text);

    void AddMessage(string text, MessageType messageType, IMessageSource? source);

    void AddErrorMessage(string text, IMessageSource? source);

    void AddWarningMessage(string text, IMessageSource? source);

    void AddInfoMessage(string text, IMessageSource? source);

    void AddDebugMessage(string text, IMessageSource? source);

    void ClearMessages();
}