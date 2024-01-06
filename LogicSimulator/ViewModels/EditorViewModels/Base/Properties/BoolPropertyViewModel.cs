namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

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

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        var firstObj = objects.First();

        IsValueUndefined = objects.Any(o => !Equals(GetValue<bool>(o), GetValue<bool>(firstObj)));

        return GetValue<bool>(firstObj);
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
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