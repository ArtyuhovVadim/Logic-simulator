namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class EnumPropertyViewModel : SinglePropertyViewModel
{
    #region IsValueUndefined

    private bool _isValueUndefined;

    public bool IsValueUndefined
    {
        get => _isValueUndefined;
        set => Set(ref _isValueUndefined, value);
    }

    #endregion

    protected override object GetPropertyValue(IReadOnlyCollection<object> objects)
    {
        var firstObjValue = GetValue<Enum>(objects.First());

        IsValueUndefined = objects.Any(o => !Equals(GetValue<Enum>(o), firstObjValue));

        return firstObjValue;
    }

    protected override void SetPropertyValue(IReadOnlyCollection<object> objects, object value)
    {
        IsValueUndefined = false;

        var valueType = value.GetType();

        foreach (var obj in objects)
        {
            SetValue(obj, value, valueType);
        }
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new EnumPropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };
}