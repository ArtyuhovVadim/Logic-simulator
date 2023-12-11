using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels;
using LogicSimulator.ViewModels.AnchorableViewModels;

namespace LogicSimulator.Infrastructure.Services;

public class SchemeViewModelFactory : ISchemeViewModelFactory
{
    private readonly DockingViewModel _dockingViewModel;
    private readonly IEditorSelectionService _selectionService;

    public SchemeViewModelFactory(DockingViewModel dockingViewModel, IEditorSelectionService selectionService)
    {
        _dockingViewModel = dockingViewModel;
        _selectionService = selectionService;
    }

    public SchemeViewModel Create(Scheme schemeModel) => new(schemeModel, _dockingViewModel, _selectionService);
}