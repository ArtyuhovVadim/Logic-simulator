using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LogicSimulator.Scene;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class BaseEditorViewModel<T> : AbstractEditorViewModel where T : BaseSceneObject
{
    #region Objects

    private ObservableCollection<T> _objects;

    protected ObservableCollection<T> Objects
    {
        get => _objects;
        private set => Set(ref _objects, value);
    }

    #endregion

    public Dictionary<string, bool> UndefinedPropertiesMap { get; } = new();

    private readonly List<string> _propertyNames = new();

    public T FirstObject { get; private set; }

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
        OnPropertyChanged(nameof(FirstObject));

        if (!UndefinedPropertiesMap.Any())
        {
            foreach (var prop in typeof(T).GetProperties().Where(x => Attribute.IsDefined(x, typeof(EditableAttribute))))
            {
                if (prop.PropertyType == typeof(Vector2))
                {
                    UndefinedPropertiesMap.Add(prop.Name + "X", false);
                    UndefinedPropertiesMap.Add(prop.Name + "Y", false);
                }
                else
                {
                    UndefinedPropertiesMap.Add(prop.Name, false);
                }

                _propertyNames.Add(prop.Name);
            }
        }

        foreach (var propName in _propertyNames)
        {
            OnPropertyChanged(propName);
        }

        OnPropertyChanged(nameof(UndefinedPropertiesMap));
    }

    protected TProp Get<TProp>([CallerMemberName] string propertyName = null)
    {
        var propInfo = typeof(T).GetProperty(propertyName!)!;

        if (propInfo.PropertyType == typeof(Vector2))
        {
            UndefinedPropertiesMap[propertyName + "X"] = Objects.Any(o => !MathUtil.NearEqual(((Vector2)propInfo.GetValue(o)!).X, ((Vector2)propInfo.GetValue(FirstObject)!).X));
            UndefinedPropertiesMap[propertyName + "Y"] = Objects.Any(o => !MathUtil.NearEqual(((Vector2)propInfo.GetValue(o)!).Y, ((Vector2)propInfo.GetValue(FirstObject)!).Y));
        }
        else
        {
            UndefinedPropertiesMap[propertyName] = Objects.Any(o => !Equals(propInfo.GetValue(o), propInfo.GetValue(FirstObject)));
        }

        OnPropertyChanged(nameof(UndefinedPropertiesMap));

        return (TProp)propInfo.GetValue(FirstObject);
    }

    protected void Set<TProp>(TProp value, [CallerMemberName] string propertyName = null)
    {
        var propInfo = typeof(T).GetProperty(propertyName!)!;

        if (propInfo.PropertyType == typeof(Vector2))
        {
            var newVec = (Vector2)(object)value;

            foreach (var o in Objects)
            {
                var oldVec = (Vector2)propInfo.GetValue(o)!;

                if (!UndefinedPropertiesMap[propertyName! + "X"]) oldVec.X = newVec.X;
                if (!UndefinedPropertiesMap[propertyName! + "Y"]) oldVec.Y = newVec.Y;

                propInfo.SetValue(o, oldVec);
            }

            return;
        }

        if (UndefinedPropertiesMap[propertyName!]) return;

        foreach (var o in Objects)
        {
            propInfo.SetValue(o, value);
        }
    }
}