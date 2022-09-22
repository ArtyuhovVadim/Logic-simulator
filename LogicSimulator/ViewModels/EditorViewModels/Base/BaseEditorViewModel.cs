using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LogicSimulator.Scene.SceneObjects.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class BaseEditorViewModel<T> : AbstractEditorViewModel where T : BaseSceneObject
{
    #region Objects

    private ObservableCollection<T> _objects;

    public ObservableCollection<T> Objects
    {
        get => _objects;
        protected set => Set(ref _objects, value);
    }

    #endregion

    protected T FirstObject { get; set; }

    public override void SetObjectsToEdit(IEnumerable<BaseSceneObject> objects)
    {
        if (Objects is not null && Objects.Any())
        {
            foreach (var o in Objects)
            {
                o.PropertyChanged -= OnPropertyChanged;
            }

            FirstObject = null;
            Objects.Clear();
        }

        if(!objects.Any())
            return;

        if (objects.Any(x => x.GetType() != typeof(T)))
            throw new ArgumentException($"All objects must be {typeof(T)} type!");

        var castObjects = objects.Cast<T>();

        Objects = new ObservableCollection<T>(castObjects);

        foreach (var o in Objects)
        {
            o.PropertyChanged += OnPropertyChanged;
        }

        FirstObject = Objects.First();

        NotifyPropertiesChange();
    }

    protected abstract void NotifyPropertiesChange();
}