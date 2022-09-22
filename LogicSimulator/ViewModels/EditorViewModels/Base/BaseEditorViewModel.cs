using System;
using System.Collections.Generic;
using System.Linq;
using LogicSimulator.Scene.SceneObjects.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class BaseEditorViewModel<T> : AbstractEditorViewModel where T : BaseSceneObject
{
    #region Objects

    private List<T> _objects;

    public List<T> Objects
    {
        get => _objects;
        protected set => Set(ref _objects, value);
    }

    #endregion

    public override void SetObjectsToEdit(IEnumerable<BaseSceneObject> objects)
    {
        if (Objects is not null && Objects.Any())
        {
            foreach (var o in Objects)
            {
                o.PropertyChanged -= OnPropertyChanged;
            }

            Objects.Clear();
        }

        if (objects.Any(x => x.GetType() != typeof(T)))
            throw new ArgumentException($"All objects must be {typeof(T)} type!");

        var castObjects = objects.Cast<T>();

        Objects = new List<T>(castObjects);

        foreach (var o in Objects)
        {
            o.PropertyChanged += OnPropertyChanged;
        }
    }
}