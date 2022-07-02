﻿using Microsoft.Extensions.DependencyInjection;

namespace LogicSimulator.Infrastructure.Services;

public static class Registrator
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserDialogService, UserDialogService>()
            ;

        return services;
    }
}