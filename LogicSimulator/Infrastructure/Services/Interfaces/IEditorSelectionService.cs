using LogicSimulator.ViewModels;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IEditorSelectionService
{
    void Select(SchemeViewModel selectedSceneObjects);
}