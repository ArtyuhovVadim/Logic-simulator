using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.ViewModels.UserDialogViewModels.Base;

namespace LogicSimulator.ViewModels.UserDialogViewModels;

public class ErrorDialogWindowViewModel : BaseUserDialogViewModel
{
    #region OkButtonClickedCommand

    private ICommand _okButtonClickedCommand;

    public ICommand OkButtonClickedCommand => _okButtonClickedCommand ??= new LambdaCommand(_ =>
    {
        OnCompleted();
    }, _ => true);

    #endregion
}