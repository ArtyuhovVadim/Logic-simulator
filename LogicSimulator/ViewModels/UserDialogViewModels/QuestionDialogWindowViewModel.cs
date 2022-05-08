using System.Windows.Input;
using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.ViewModels.UserDialogViewModels.Base;

namespace LogicSimulator.ViewModels.UserDialogViewModels;

public class QuestionDialogWindowViewModel : BaseUserDialogViewModel
{
    #region ConfirmCommand

    private ICommand _confirmCommand;

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommand(_ =>
    {
        OnCompleted(UserDialogResult.Yes);
    }, _ => true);

    #endregion

    #region RejectCommand

    private ICommand _rejectCommand;

    public ICommand RejectCommand => _rejectCommand ??= new LambdaCommand(_ =>
    {
        OnCompleted(UserDialogResult.No);
    }, _ => true);

    #endregion
}