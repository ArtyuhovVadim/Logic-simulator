using System.Windows;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Common;
using LogicSimulator.ViewModels.Anchorable;
using LogicSimulator.ViewModels.Common;

namespace LogicSimulator.Infrastructure.Services;

public class OutputMessagesService : IOutputMessagesService
{
    private readonly MessagesOutputViewModel _messagesOutputViewModel;

    public OutputMessagesService(MessagesOutputViewModel messagesOutputViewModel) => _messagesOutputViewModel = messagesOutputViewModel;

    public void AddErrorMessage(string text) => AddErrorMessage(text, null);

    public void AddWarningMessage(string text) => AddWarningMessage(text, null);

    public void AddInfoMessage(string text) => AddInfoMessage(text, null);

    public void AddDebugMessage(string text) => AddDebugMessage(text, null);

    public void AddErrorMessage(string text, IMessageSource? source) => AddMessage(new OutputMessageViewModel { Text = text, Type = MessageType.Error, Time = DateTime.Now, Source = source });

    public void AddWarningMessage(string text, IMessageSource? source) => AddMessage(new OutputMessageViewModel { Text = text, Type = MessageType.Warning, Time = DateTime.Now, Source = source });

    public void AddInfoMessage(string text, IMessageSource? source) => AddMessage(new OutputMessageViewModel { Text = text, Type = MessageType.Information, Time = DateTime.Now, Source = source });

    public void AddDebugMessage(string text, IMessageSource? source)
    {
        if (!App.IsDevelopment) return;

        AddMessage(new OutputMessageViewModel { Text = text, Type = MessageType.Debug, Time = DateTime.Now, Source = source });
    }

    public void ClearMessages() => Application.Current.Dispatcher.BeginInvoke(() => _messagesOutputViewModel.Messages.Clear());

    private void AddMessage(OutputMessageViewModel message) => Application.Current.Dispatcher.BeginInvoke(() => _messagesOutputViewModel.Messages.Add(message));
}