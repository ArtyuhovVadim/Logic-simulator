using System;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.UserDialogViewModels;

public class InfoDialogWindowViewModel : BindableBase
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

    #region OkButtonClickedCommand

    private ICommand _okButtonClickedCommand;

    public ICommand OkButtonClickedCommand => _okButtonClickedCommand ??= new LambdaCommand(_ =>
    {
        Completed?.Invoke();
    }, _ => true);

    #endregion
}