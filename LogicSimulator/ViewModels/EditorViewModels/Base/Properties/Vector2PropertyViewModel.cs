using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class Vector2PropertyViewModel : SinglePropertyViewModel
{
    #region IsXUndedined

    private bool _isXUndefined;

    public bool IsXUndefined
    {
        get => _isXUndefined;
        set => Set(ref _isXUndefined, value);
    }

    #endregion

    #region IsYUndefinded

    private bool _isYUndefined;

    public bool IsYUndefined
    {
        get => _isYUndefined;
        set => Set(ref _isYUndefined, value);
    }

    #endregion

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        var firstObj = objects.First();

        IsXUndefined = objects.Any(o => !MathUtil.NearEqual(GetValue<Vector2>(o).X, GetValue<Vector2>(firstObj).X));
        IsYUndefined = objects.Any(o => !MathUtil.NearEqual(GetValue<Vector2>(o).Y, GetValue<Vector2>(firstObj).Y));

        return GetValue<Vector2>(firstObj);
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        foreach (var obj in objects)
        {
            var oldVec = GetValue<Vector2>(obj);

            var newVector = (Vector2)value;

            if (!IsXUndefined) oldVec.X = newVector.X;
            if (!IsYUndefined) oldVec.Y = newVector.Y;

            SetValue(obj, oldVec);
        }
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new Vector2PropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };
}