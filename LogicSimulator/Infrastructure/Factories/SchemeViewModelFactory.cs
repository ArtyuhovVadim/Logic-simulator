using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Models.Base;
using LogicSimulator.ViewModels;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.Infrastructure.Factories;

public class SchemeViewModelFactory : ISchemeViewModelFactory
{
    private readonly DockingViewModel _dockingViewModel;
    private readonly IEditorSelectionService _selectionService;
    private readonly ISchemeSimulatorService _schemeSimulatorService;
    private readonly ISchemeBuilderService _schemeBuilderService;
    private readonly IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel> _viewModelsFactory;

    public SchemeViewModelFactory(DockingViewModel dockingViewModel,
                                  IEditorSelectionService selectionService,
                                  ISchemeSimulatorService schemeSimulatorService,
                                  ISchemeBuilderService schemeBuilderService,
                                  IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel> viewModelsFactory)
    {
        _dockingViewModel = dockingViewModel;
        _selectionService = selectionService;
        _schemeSimulatorService = schemeSimulatorService;
        _schemeBuilderService = schemeBuilderService;
        _viewModelsFactory = viewModelsFactory;
    }

    public SchemeViewModel Create(Scheme schemeModel) => new(schemeModel, _dockingViewModel, _selectionService, _schemeSimulatorService, _schemeBuilderService, _viewModelsFactory);
}