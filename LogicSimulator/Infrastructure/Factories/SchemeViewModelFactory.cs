using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Models.Objects.Base;
using LogicSimulator.ViewModels;
using LogicSimulator.ViewModels.Anchorable;
using LogicSimulator.ViewModels.Objects.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LogicSimulator.Infrastructure.Factories;

public class SchemeViewModelFactory : ISchemeViewModelFactory
{
    private readonly IServiceProvider _provider;

    public SchemeViewModelFactory(IServiceProvider provider) => _provider = provider;

    public SchemeViewModel Create(Scheme schemeModel) => new(
        schemeModel,
        _provider.GetRequiredService<DockingViewModel>(),
        _provider.GetRequiredService<TimelineViewModel>(),
        _provider.GetRequiredService<IEditorSelectionService>(),
        _provider.GetRequiredService<ISchemeSimulatorService>(),
        _provider.GetRequiredService<ISchemeBuilderService>(),
        _provider.GetRequiredService<IToolSwitcherService>(),
        _provider.GetRequiredService<IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel>>(),
        _provider.GetRequiredService<IOutputMessagesService>(),
        _provider.GetRequiredService<ILogger<SchemeViewModel>>());
}