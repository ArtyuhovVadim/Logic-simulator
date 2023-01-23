using System;
using System.Collections.Generic;
using System.Linq;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class FloatPropertyViewModel : PropertyViewModel
{
    #region IsValueUndefined

    private bool _isValueUndefined;

    public bool IsValueUndefined
    {
        get => _isValueUndefined;
        set => Set(ref _isValueUndefined, value);
    }

    #endregion

    public FloatPropertyViewModel(string propertyName, Func<IEnumerable<object>> getObjectsFunc) : base(propertyName, typeof(float), getObjectsFunc) { }

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        var propInfo = objects.First().GetType().GetProperty(PropertyName)!;
        IsValueUndefined = objects.Any(o => !Equals(propInfo.GetValue(o), propInfo.GetValue(objects.First())));
        return Convert.ToDouble(propInfo.GetValue(objects.First()));
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        var propInfo = objects.First().GetType().GetProperty(PropertyName)!;

        if (IsValueUndefined) return;

        foreach (var o in objects)
        {
            propInfo.SetValue(o, Convert.ToSingle(value));
        }
    }
}

public class PropertyViewModel : BindableBase
{
    private readonly Func<IEnumerable<object>> _getObjectsFunc;

    #region Value

    public object Value
    {
        get => GetPropertyValue(Objects);
        set
        {
            SetPropertyValue(Objects, value);
            OnPropertyChanged();
        }
    }

    #endregion

    public string PropertyName { get; set; }

    public Type PropertyType { get; set; }

    protected IEnumerable<object> Objects => _getObjectsFunc();

    public PropertyViewModel(string propertyName, Type propertyType, Func<IEnumerable<object>> getObjectsFunc)
    {
        _getObjectsFunc = getObjectsFunc;
        PropertyName = propertyName;
        PropertyType = propertyType;
    }

    protected virtual object GetPropertyValue(IEnumerable<object> objects)
    {
        return null;
    }

    protected virtual void SetPropertyValue(IEnumerable<object> objects, object value)
    {

    }

    public void ProvidePropertyChanged(string propName)
    {
        if (PropertyName == propName)
        {
            OnPropertyChanged(nameof(Value));
        }
    }
}