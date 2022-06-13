using System;
using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.UserDialogViewModels.Base;

public abstract class BaseUserDialogViewModel : BindableBase
{
    public event Action<UserDialogResult> Completed;

    #region Title

    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    #endregion

    #region Message

    private string _message = string.Empty;
    public string Message
    {
        get => _message;
        set => Set(ref _message, value);
    }

    #endregion

    protected void OnCompleted(UserDialogResult result)
    {
        Completed?.Invoke(result);
    }
}