namespace LogicSimulator.ViewModels.Editors.Base.Properties;

public class StringPropertyViewModel : SinglePropertyViewModel
{
    public bool IsMultiline { get; set; } = false;

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
        var firstObjValue = GetValue<string>(objects.First());

        IsValueUndefined = objects.Any(o => !Equals(GetValue<string>(o), firstObjValue));

        return firstObjValue;
    }

    protected override void SetPropertyValue(IReadOnlyCollection<object> objects, object value)
    {
        IsValueUndefined = false;

        var newValue = (string)value;

        foreach (var obj in objects)
        {
            SetValue(obj, newValue);
        }
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new StringPropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };
}