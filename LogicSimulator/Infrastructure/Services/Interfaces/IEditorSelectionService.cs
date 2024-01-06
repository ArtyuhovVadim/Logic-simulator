using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IEditorSelectionService
{
    void SetObjectsEditor(ICollection<BaseObjectViewModel> objects);

    void SetSchemeEditor(SchemeViewModel schemeViewModel);

    void SetEmptyEditor();
}