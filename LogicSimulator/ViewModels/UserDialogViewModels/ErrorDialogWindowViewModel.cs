using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.UserDialogViewModels.Base;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.UserDialogViewModels;

public class ErrorDialogWindowViewModel : BaseUserDialogViewModel
{
    #region ConfirmCommand

    private ICommand _confirmCommand;

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommand(_ =>
    {
        OnCompleted(UserDialogResult.Ok);
    });

    #endregion
}