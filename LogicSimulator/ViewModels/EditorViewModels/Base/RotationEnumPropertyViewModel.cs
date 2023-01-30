using LogicSimulator.Scene;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public class RotationEnumPropertyViewModel : SinglePropertyViewModel
{
    private string _degrees0String = "0 Градусов";
    private string _degrees90String = "90 Градусов";
    private string _degrees180String = "180 Градусов";
    private string _degrees270String = "270 Градусов";

    #region IsValueUndefined

    private bool _isValueUndefined;

    public bool IsValueUndefined
    {
        get => _isValueUndefined;
        set => Set(ref _isValueUndefined, value);
    }

    #endregion

    public IEnumerable<string> Rotations { get; }

    public RotationEnumPropertyViewModel()
    {
        Rotations = new[]
        {
            _degrees0String,
            _degrees90String,
            _degrees180String,
            _degrees270String,
        };
    }

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        IsValueUndefined = objects.Any(o => !Equals(PropertyInfo.GetValue(o), PropertyInfo.GetValue(objects.First())));
        return RotationToString((Rotation)PropertyInfo.GetValue(objects.First()));
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        value = StringToRotation((string)value);

        if (IsValueUndefined) return;

        foreach (var obj in objects)
        {
            PropertyInfo.SetValue(obj, value);
        }
    }

    private string RotationToString(Rotation rotation) => rotation switch
    {
        Rotation.Degrees0 => _degrees0String,
        Rotation.Degrees90 => _degrees90String,
        Rotation.Degrees180 => _degrees180String,
        Rotation.Degrees270 => _degrees270String,
        _ => "Undefined"
    };

    private Rotation StringToRotation(string str)
    {
        if (str == _degrees0String) return Rotation.Degrees0;
        if (str == _degrees90String) return Rotation.Degrees90;
        if (str == _degrees180String) return Rotation.Degrees180;
        if (str == _degrees270String) return Rotation.Degrees270;

        return Rotation.Undefined;
    }
}