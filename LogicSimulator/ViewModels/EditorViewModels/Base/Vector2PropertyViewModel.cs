using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public class Vector2PropertyViewModel : PropertyViewModel
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

    public Vector2PropertyViewModel() : base(typeof(Vector2)) { }

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        IsXUndefined = objects.Any(o => !MathUtil.NearEqual(((Vector2)PropertyInfo.GetValue(o)!).X, ((Vector2)PropertyInfo.GetValue(objects.First())!).X));
        IsYUndefined = objects.Any(o => !MathUtil.NearEqual(((Vector2)PropertyInfo.GetValue(o)!).Y, ((Vector2)PropertyInfo.GetValue(objects.First())!).Y));

        return PropertyInfo.GetValue(objects.First());
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        var newVec = (Vector2)value;

        foreach (var obj in objects)
        {
            var oldVec = (Vector2)PropertyInfo.GetValue(obj)!;

            if (!IsXUndefined) oldVec.X = newVec.X;
            if (!IsYUndefined) oldVec.Y = newVec.Y;

            PropertyInfo.SetValue(obj, oldVec);
        }
    }
}