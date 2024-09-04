using LogicSimulator.ViewModels.Dialog.Base;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.Dialog;

public class ErrorDialogViewModel : BaseDialogViewModel
{
    #region Message

    private string _message = string.Empty;

    public string Message
    {
        get => _message;
        set => Set(ref _message, value);
    }

    #endregion

    #region OkCommand

    private ICommand? _okCommand;

    public ICommand OkCommand => _okCommand ??= new LambdaCommand(OnCompleted);

    #endregion
}