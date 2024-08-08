namespace LogicSimulator.ViewModels.Editors.Base.Properties;

public class BoolPropertyViewModel : SinglePropertyViewModel
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
        var firstObjValue = GetValue<bool>(objects.First());

        IsValueUndefined = objects.Any(o => !Equals(GetValue<bool>(o), firstObjValue));

        return firstObjValue;
    }

    protected override void SetPropertyValue(IReadOnlyCollection<object> objects, object value)
    {
        IsValueUndefined = false;

        var newValue = (bool)value;

        foreach (var obj in objects)
        {
            SetValue(obj, newValue);
        }
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new BoolPropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };
}