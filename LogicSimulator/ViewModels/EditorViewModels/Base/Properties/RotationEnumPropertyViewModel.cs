using LogicSimulator.Infrastructure;

namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class RotationEnumPropertyViewModel : SinglePropertyViewModel
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

        IsValueUndefined = objects.Any(o => !Equals(GetValue<Rotation>(o), GetValue<Rotation>(firstObj)));

        return GetValue<Rotation>(firstObj);
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        if (IsValueUndefined) return;

        var newValue = (Rotation)value;

        foreach (var obj in objects)
        {
            SetValue(obj, newValue);
        }
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new RotationEnumPropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };
}