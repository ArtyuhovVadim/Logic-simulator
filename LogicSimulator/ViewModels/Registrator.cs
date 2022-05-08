using LogicSimulator.ViewModels.UserDialogViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LogicSimulator.ViewModels;

public static class Registrator
{
    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<InfoDialogWindowViewModel>()
            .AddSingleton<ErrorDialogWindowViewModel>()
            .AddSingleton<WarningDialogWindowViewModel>()
            .AddSingleton<QuestionDialogWindowViewModel>()
            ;

        return services;
    }
}