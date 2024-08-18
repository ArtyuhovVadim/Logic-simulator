using System.ComponentModel;
using System.Windows.Data;
using LogicSimulator.Models.Common;
using LogicSimulator.Models.MessageSources;
using LogicSimulator.ViewModels.Anchorable.Base;
using LogicSimulator.ViewModels.Common;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.Anchorable;

public class MessagesOutputViewModel : ToolViewModel
{
    private readonly ICollectionView _messagesCollectionView;

    public override string Title => "Вывод";

    public MessagesOutputViewModel()
    {
        _messagesCollectionView = CollectionViewSource.GetDefaultView(Messages);
        _messagesCollectionView.Filter = OnFilterMessages;
    }

    #region Messages

    public ObservableCollection<OutputMessageViewModel> Messages { get; } = [];

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
                _messagesCollectionView.Refresh();
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
                _messagesCollectionView.Refresh();
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
                _messagesCollectionView.Refresh();
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
                _messagesCollectionView.Refresh();
            }
        }
    }

    #endregion

    #region IsDebugMessagesVisible

    private bool _isDebugMessagesVisible = true;

    public bool IsDebugMessagesVisible
    {
        get => _isDebugMessagesVisible;
        set
        {
            if (Set(ref _isDebugMessagesVisible, value))
            {
                _messagesCollectionView.Refresh();
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
        IsDebugMessagesVisible = true;
        SearchText = string.Empty;
        _messagesCollectionView.SortDescriptions.Clear();
    });

    #endregion

    #region ClearMessagesCommand

    private ICommand? _clearMessagesCommand;

    public ICommand ClearMessagesCommand => _clearMessagesCommand ??= new LambdaCommand(Messages.Clear);

    #endregion

    #region GoToMessageSourceCommand

    private ICommand? _goToMessageSourceCommand;

    public ICommand GoToMessageSourceCommand => _goToMessageSourceCommand ??= new LambdaCommand<IMessageSource>(source => source?.GoTo(), source => source is not null);

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

        if (IsDebugMessagesVisible && message.Type == MessageType.Debug)
            return true;

        return false;
    }
}