using System.Windows;
using WpfExtensions.Mvvm.Commands.Base;

namespace LogicSimulator.Infrastructure.Commands;

public class AppExitCommand : BaseCommand
{
    protected override void OnExecute(object? parameter) => Application.Current.Shutdown(Environment.ExitCode);
}