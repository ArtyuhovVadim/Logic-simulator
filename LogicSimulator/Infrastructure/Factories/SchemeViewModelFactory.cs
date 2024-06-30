using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Models.Base;
using LogicSimulator.ViewModels;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using Microsoft.Extensions.Logging;

namespace LogicSimulator.Infrastructure.Factories;

public class SchemeViewModelFactory : ISchemeViewModelFactory
{
    private readonly DockingViewModel _dockingViewModel;
    private readonly IEditorSelectionService _selectionService;
    private readonly ISchemeSimulatorService _schemeSimulatorService;
    private readonly ISchemeBuilderService _schemeBuilderService;
    private readonly IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel> _viewModelsFactory;
    private readonly ILogger<SchemeViewModel> _logger;

    public SchemeViewModelFactory(DockingViewModel dockingViewModel,
                                  IEditorSelectionService selectionService,
                                  ISchemeSimulatorService schemeSimulatorService,
                                  ISchemeBuilderService schemeBuilderService,
                                  IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel> viewModelsFactory,
                                  ILogger<SchemeViewModel> logger)
    {
        _dockingViewModel = dockingViewModel;
        _selectionService = selectionService;
        _schemeSimulatorService = schemeSimulatorService;
        _schemeBuilderService = schemeBuilderService;
        _viewModelsFactory = viewModelsFactory;
        _logger = logger;
    }

    public SchemeViewModel Create(Scheme schemeModel) => new(schemeModel, _dockingViewModel, _selectionService, _schemeSimulatorService, _schemeBuilderService, _viewModelsFactory, _logger);
}