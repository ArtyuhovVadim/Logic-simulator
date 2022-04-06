using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly IUserDialogService _userDialogService;

    public MainWindowViewModel(IUserDialogService userDialogService)
    {
        _userDialogService = userDialogService;
    }

    private ICommand _testCommand;

    public ICommand TestCommand => _testCommand ??= new LambdaCommand(_ =>
    {
       _userDialogService.ShowInfoMessage("Some info123123123");
    }, _ => true);
}