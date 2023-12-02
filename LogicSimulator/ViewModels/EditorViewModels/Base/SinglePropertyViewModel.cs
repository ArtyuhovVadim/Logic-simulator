namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class SinglePropertyViewModel : PropertyViewModel
{
    private readonly Dictionary<Type, Func<object, object>> _gettersMap = [];

    private readonly Dictionary<Type, Action<object, object>> _settersMap = [];

    public string PropertyName { get; init; }

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

    protected void SetValue<TProperty>(object obj, TProperty value) => GettersAndSettersCache.GetSetter<TProperty>(PropertyName, obj)(obj, value);

    public override void RaisePropertyChanged() => OnPropertyChanged(nameof(Value));
}