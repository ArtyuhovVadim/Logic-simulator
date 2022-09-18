using System.Collections.Generic;
using System.Linq;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public abstract class BaseEditorViewModel : BindableBase
{
    private List<BaseSceneObject> _sceneObjects;

    public List<BaseSceneObject> SceneObjects
    {
        get => _sceneObjects;
        set
        {
            _sceneObjects = value;
            SceneObjects.First().PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
        }
    }
}