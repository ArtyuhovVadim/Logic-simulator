using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class PropertyViewModel : BindableBase
{
    private PropertyInfo _propertyInfo;

    public EditorViewModel EditorViewModel { get; set; }

    public string PropertyName { get; set; }

    protected PropertyInfo PropertyInfo => _propertyInfo ??= EditorViewModel.Objects.First().GetType().GetProperty(PropertyName);

    #region Value

    public object Value
    {
        get => GetPropertyValue(EditorViewModel.Objects);
        set
        {
            SetPropertyValue(EditorViewModel.Objects, value);
            OnPropertyChanged();
        }
    }

    #endregion

    public void ProvidePropertyChanged(string propName)
    {
        if (PropertyName == propName)
        {
            OnPropertyChanged(nameof(Value));
        }
    }

    protected abstract object GetPropertyValue(IEnumerable<object> objects);

    protected abstract void SetPropertyValue(IEnumerable<object> objects, object value);
}