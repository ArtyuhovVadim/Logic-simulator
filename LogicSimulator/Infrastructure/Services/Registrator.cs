using LogicSimulator.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LogicSimulator.Infrastructure.Services;

public static class Registrator
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserDialogService, UserDialogService>()
                .AddSingleton<ISchemeFileService, SchemeFileService>()

                .AddSingleton<IPropertiesSelectionService, EditorSelectionService>()
                ;

        return services;
    }
}