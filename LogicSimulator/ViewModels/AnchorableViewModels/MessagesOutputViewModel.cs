using System.ComponentModel;
using System.Windows.Data;
using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.AnchorableViewModels;

public class MessagesOutputViewModel : ToolViewModel
{
    public override string Title => "Вывод";

    public MessagesOutputViewModel()
    {
        MessagesCollectionView = CollectionViewSource.GetDefaultView(Messages);
        MessagesCollectionView.Filter = OnFilterMessages;

        _messages.Add(new OutputMessageViewModel { Type = MessageType.Information, Text = "Тестовое информационное сообщение" });
        _messages.Add(new OutputMessageViewModel { Type = MessageType.Warning, Text = "Тестовое сообщение с предупреждением" });
        _messages.Add(new OutputMessageViewModel { Type = MessageType.Error, Text = "Тестовое сообщение с ошибкой" });
        _messages.Add(new OutputMessageViewModel { Type = MessageType.Information, Text = "Тестовое информационное сообщение" });
        _messages.Add(new OutputMessageViewModel { Type = MessageType.Warning, Text = "Тестовое сообщение с предупреждением" });
        _messages.Add(new OutputMessageViewModel { Type = MessageType.Error, Text = "Тестовое сообщение с ошибкой" });
        _messages.Add(new OutputMessageViewModel { Type = MessageType.Information, Text = "Тестовое информационное сообщение" });
        _messages.Add(new OutputMessageViewModel { Type = MessageType.Warning, Text = "Тестовое сообщение с предупреждением" });
        _messages.Add(new OutputMessageViewModel { Type = MessageType.Error, Text = "Тестовое сообщение с ошибкой" });
    }

    #region MessagesCollectionView

    public ICollectionView MessagesCollectionView { get; }

    #endregion

    #region Messages

    private readonly ObservableCollection<OutputMessageViewModel> _messages = [];

    public IEnumerable<OutputMessageViewModel> Messages => _messages;

    #endregion

    #region SearchText

    private string _searchText = string.Empty;

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (Set(ref _searchText, value))
            {
                MessagesCollectionView.Refresh();
            }
        }
    }

    #endregion

    #region IsErrorMessagesVisible

    private bool _isErrorMessagesVisible = true;

    public bool IsErrorMessagesVisible
    {
        get => _isErrorMessagesVisible;
        set
        {
            if (Set(ref _isErrorMessagesVisible, value))
            {
                MessagesCollectionView.Refresh();
            }
        }
    }

    #endregion

    #region IsWarningMessagesVisible

    private bool _isWarningMessagesVisible = true;

    public bool IsWarningMessagesVisible
    {
        get => _isWarningMessagesVisible;
        set
        {
            if (Set(ref _isWarningMessagesVisible, value))
            {
                MessagesCollectionView.Refresh();
            }
        }
    }

    #endregion

    #region IsInformationMessagesVisible

    private bool _isInformationMessagesVisible = true;

    public bool IsInformationMessagesVisible
    {
        get => _isInformationMessagesVisible;
        set
        {
            if (Set(ref _isInformationMessagesVisible, value))
            {
                MessagesCollectionView.Refresh();
            }
        }
    }

    #endregion

    #region ClearFiltersCommand

    private ICommand? _clearFiltersCommand;

    public ICommand ClearFiltersCommand => _clearFiltersCommand ??= new LambdaCommand(() =>
    {
        IsErrorMessagesVisible = true;
        IsInformationMessagesVisible = true;
        IsWarningMessagesVisible = true;
        SearchText = string.Empty;
    });

    #endregion

    private bool OnFilterMessages(object obj)
    {
        var message = (OutputMessageViewModel)obj;

        if (!message.Text.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase))
            return false;

        if (IsErrorMessagesVisible && message.Type == MessageType.Error)
            return true;

        if (IsWarningMessagesVisible && message.Type == MessageType.Warning)
            return true;

        if (IsInformationMessagesVisible && message.Type == MessageType.Information)
            return true;

        return false;
    }
}