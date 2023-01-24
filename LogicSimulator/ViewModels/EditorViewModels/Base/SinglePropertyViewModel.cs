using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class SinglePropertyViewModel : PropertyViewModel
{
    private PropertyInfo _propertyInfo;

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

    public override void ProvidePropertyChanged(string propName)
    {
        if (PropertyName == propName)
        {
            OnPropertyChanged(nameof(Value));
        }
    }

    protected abstract object GetPropertyValue(IEnumerable<object> objects);

    protected abstract void SetPropertyValue(IEnumerable<object> objects, object value);
}