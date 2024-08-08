using System.Windows.Media;

namespace LogicSimulator.ViewModels.Editors.Base.Properties;

public class ColorPropertyViewModel : SinglePropertyViewModel
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
        var firstObjValue = GetValue<Color>(objects.First());

        IsValueUndefined = objects.Any(o => !Equals(GetValue<Color>(o), firstObjValue));

        return firstObjValue;
    }

    protected override void SetPropertyValue(IReadOnlyCollection<object> objects, object value)
    {
        IsValueUndefined = false;

        var newValue = (Color)value;

        foreach (var obj in objects)
        {
            SetValue(obj, newValue);
        }
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new ColorPropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };
}