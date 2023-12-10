namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class FloatPropertyViewModel : SinglePropertyViewModel
{
    #region IsValueUndefined

    private bool _isValueUndefined;

    public bool IsValueUndefined
    {
        get => _isValueUndefined;
        set => Set(ref _isValueUndefined, value);
    }

    #endregion

    #region MaxNumber

    private float _maxNumber = float.MaxValue;

    public float MaxNumber
    {
        get => _maxNumber;
        set => Set(ref _maxNumber, value);
    }

    #endregion

    #region MinNumber

    private float _minNumber = float.MinValue;

    public float MinNumber
    {
        get => _minNumber;
        set => Set(ref _minNumber, value);
    }

    #endregion

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        var firstObj = objects.First();

        IsValueUndefined = objects.Any(o => !Equals(GetValue<float>(o), GetValue<float>(firstObj)));

        return Convert.ToDouble(GetValue<float>(firstObj));
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        if (IsValueUndefined) return;

        var newValue = Convert.ToSingle(value);

        foreach (var obj in objects)
        {
            SetValue(obj, newValue);
        }
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new FloatPropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };
}