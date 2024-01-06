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

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        var firstObj = objects.First();

        IsValueUndefined = objects.Any(o => !Equals(GetValue<Enum>(o), GetValue<Enum>(firstObj)));

        return GetValue<Enum>(firstObj);
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
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