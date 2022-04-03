using Microsoft.Extensions.DependencyInjection;

namespace LogicSimulator.ViewModels;

public static class Registrator
{
    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services
            .AddSingleton<MainWindowViewModel>()
            ;

        return services;
    }
}