using LogicSimulator.ViewModels.Anchorable;
using LogicSimulator.ViewModels.Objects.Base;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IEditorSelectionService
{
    void SetObjectsEditor(ICollection<BaseObjectViewModel> objects);

    void SetSchemeEditor(SchemeViewModel schemeViewModel);

    void SetEmptyEditor();
}