using System;
using System.Windows;
using LogicSimulator.Infrastructure.Commands.Base;

namespace LogicSimulator.Infrastructure.Commands;

public class AppExitCommand : Command
{
    public override bool CanExecute(object parameter)
    {
        return true;
    }

    public override void Execute(object parameter)
    {
        Application.Current.Shutdown(Environment.ExitCode);
    }
}