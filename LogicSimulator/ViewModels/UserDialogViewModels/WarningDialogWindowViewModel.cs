﻿using System.Windows.Input;
using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.ViewModels.UserDialogViewModels.Base;

namespace LogicSimulator.ViewModels.UserDialogViewModels;

public class WarningDialogWindowViewModel : BaseUserDialogViewModel
{
    #region ConfirmCommand

    private ICommand _confirmCommand;

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommand(_ =>
    {
        OnCompleted(UserDialogResult.Ok);
    }, _ => true);

    #endregion
}