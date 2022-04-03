using Microsoft.Extensions.DependencyInjection;

namespace LogicSimulator.ViewModels;

public class ViewModelLocator
{
    public MainWindowViewModel MainWindowViewModel => App.Host.Services.GetRequiredService<MainWindowViewModel>();
}