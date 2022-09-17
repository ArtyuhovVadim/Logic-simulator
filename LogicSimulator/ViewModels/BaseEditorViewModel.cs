using System.Collections.Generic;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public abstract class BaseEditorViewModel : BindableBase
{
    public List<BaseSceneObject> SceneObjects;

}