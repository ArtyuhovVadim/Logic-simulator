using System.Windows;
using WpfExtensions.Mvvm.Commands.Base;

namespace LogicSimulator.Infrastructure.Commands;

public class CopyToClipboardCommand : BaseCommand
{
    protected override void OnExecute(object? parameter) => Clipboard.SetText(parameter!.ToString()!, TextDataFormat.UnicodeText);

    protected override bool OnCanExecute(object? parameter) => parameter is not null;
}