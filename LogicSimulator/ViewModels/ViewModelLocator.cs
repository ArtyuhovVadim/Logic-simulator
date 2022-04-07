using LogicSimulator.ViewModels.UserDialogViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LogicSimulator.ViewModels;

public class ViewModelLocator
{
    public MainWindowViewModel MainWindowViewModel => App.Host.Services.GetRequiredService<MainWindowViewModel>();

    public InfoDialogWindowViewModel InfoDialogWindowViewModel => App.Host.Services.GetRequiredService<InfoDialogWindowViewModel>();

    public ErrorDialogWindowViewModel ErrorDialogWindowViewModel => App.Host.Services.GetRequiredService<ErrorDialogWindowViewModel>();
}