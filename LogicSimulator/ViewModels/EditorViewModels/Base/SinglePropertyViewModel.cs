namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class SinglePropertyViewModel : PropertyViewModel
{
    public string PropertyName { get; init; } = string.Empty;

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

    protected TProperty GetValue<TProperty>(object obj) => (TProperty)GettersAndSettersCache.GetGetter(PropertyName, obj)(obj);

    protected void SetValue<TProperty>(object obj, TProperty value) => GettersAndSettersCache.GetSetter<TProperty>(PropertyName, obj)(obj, value!);

    public override void RaisePropertyChanged() => OnPropertyChanged(nameof(Value));
}