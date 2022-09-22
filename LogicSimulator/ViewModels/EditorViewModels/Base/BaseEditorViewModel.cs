using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LogicSimulator.Scene;
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

    public Dictionary<string, bool> UndefinedPropertiesMap { get; } = new();

    private T FirstObject { get; set; }

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

        if (!objects.Any()) return;

        if (objects.Any(x => x.GetType() != typeof(T)))
            throw new ArgumentException($"All objects must be {typeof(T)} type!");

        var castObjects = objects.Cast<T>();

        Objects = new ObservableCollection<T>(castObjects);

        foreach (var o in Objects)
        {
            o.PropertyChanged += OnPropertyChanged;
        }

        FirstObject = Objects.First();

        if (!UndefinedPropertiesMap.Any())
        {
            foreach (var prop in typeof(T).GetProperties().Where(x => Attribute.IsDefined(x, typeof(EditableAttribute))))
            {
                UndefinedPropertiesMap.Add(prop.Name, false);
            }
        }

        foreach (var key in UndefinedPropertiesMap.Keys)
        {
            OnPropertyChanged(key);
        }

        OnPropertyChanged(nameof(UndefinedPropertiesMap));
    }

    protected TProp Get<TProp>([CallerMemberName] string propertyName = null)
    {
        var propInfo = typeof(T).GetProperty(propertyName!)!;

        UndefinedPropertiesMap[propertyName] = Objects.Any(o => !Equals(propInfo.GetValue(o), propInfo.GetValue(FirstObject)));

        OnPropertyChanged(nameof(UndefinedPropertiesMap));

        return (TProp)propInfo.GetValue(FirstObject);
    }

    protected void Set<TProp>(TProp value, [CallerMemberName] string propertyName = null)
    {
        if (UndefinedPropertiesMap[propertyName!]) return;

        var propInfo = typeof(T).GetProperty(propertyName!)!;

        foreach (var o in Objects)
        {
            propInfo.SetValue(o, value);
        }
    }
}