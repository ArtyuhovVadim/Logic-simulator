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
    private readonly IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel> _viewModelsFactory;

    public SchemeViewModelFactory(DockingViewModel dockingViewModel, IEditorSelectionService selectionService, IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel> viewModelsFactory)
    {
        _dockingViewModel = dockingViewModel;
        _selectionService = selectionService;
        _viewModelsFactory = viewModelsFactory;
    }

    public SchemeViewModel Create(Scheme schemeModel) => new(schemeModel, _dockingViewModel, _selectionService, _viewModelsFactory);
}