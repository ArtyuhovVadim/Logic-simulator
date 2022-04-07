using System;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.UserDialogViewModels.Base;

public abstract class BaseUserDialogViewModel : BindableBase
{
    public event Action Completed;
    
    #region Message

    private string _message = string.Empty;
    public string Message
    {
        get => _message;
        set => Set(ref _message, value);
    }

    #endregion

    protected void OnCompleted()
    {
        Completed?.Invoke();
    }
}