using Microsoft.Extensions.DependencyInjection;

namespace LogicSimulator.ViewModels.Infrastructure;

public class ViewModelLocator
{
    public static MainWindowViewModel MainWindowViewModel => App.Host.Services.GetRequiredService<MainWindowViewModel>();
}