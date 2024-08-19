using LogicSimulator.Infrastructure.Messages;
using LogicSimulator.ViewModels.Anchorable.Base;
using WpfExtensions.Mvvm.Messaging;

namespace LogicSimulator.Models.MessageSources;

public class DocumentMessageSource : IMessageSource
{
    private readonly IMessageBus _messageBus;
    private readonly DocumentViewModel _document;

    public DocumentMessageSource(IMessageBus messageBus, DocumentViewModel document)
    {
        _messageBus = messageBus;
        _document = document;
    }

    public string Name => _document.Title;

    public void GoTo() => _messageBus.Send(new DocumentOpenedMessage(_document));
}
