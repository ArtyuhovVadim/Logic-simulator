using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.UserDialogViewModels.Base;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.UserDialogViewModels;

public class QuestionDialogWindowViewModel : BaseUserDialogViewModel
{
    #region ConfirmCommand

    private ICommand _confirmCommand;

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommand(_ =>
    {
        OnCompleted(UserDialogResult.Yes);
    });

    #endregion

    #region RejectCommand

    private ICommand _rejectCommand;

    public ICommand RejectCommand => _rejectCommand ??= new LambdaCommand(_ =>
    {
        OnCompleted(UserDialogResult.No);
    }, _ => true);

    #endregion
}