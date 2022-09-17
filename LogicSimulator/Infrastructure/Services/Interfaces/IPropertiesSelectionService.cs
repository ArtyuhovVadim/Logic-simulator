using System.Collections.Generic;
using LogicSimulator.Scene.SceneObjects.Base;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IPropertiesSelectionService
{
    void Select(IEnumerable<BaseSceneObject> selectedSceneObjects);
}