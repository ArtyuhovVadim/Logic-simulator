﻿using LogicSimulator.ViewModels.UserDialogViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LogicSimulator.ViewModels.Infrastructure;

public static class Registrator
{
    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services
            .AddSingleton<MainWindowViewModel>()

            .AddSingleton<PropertiesViewModel>()

            .AddTransient<RectangleEditorViewModel>()

            .AddSingleton<InfoDialogWindowViewModel>()
            .AddSingleton<ErrorDialogWindowViewModel>()
            .AddSingleton<WarningDialogWindowViewModel>()
            .AddSingleton<QuestionDialogWindowViewModel>()
            ;

        return services;
    }
}