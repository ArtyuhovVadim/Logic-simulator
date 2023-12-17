﻿using System.Globalization;
using LogicSimulator.Utils;
using MathExpressionParser;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class Vector2PropertyViewModel : SinglePropertyViewModel
{
    private static readonly MathParser Parser = MathParserBuilder.BuildDefaultParser();

    private ChangedVectorComponent _changedVectorComponent = ChangedVectorComponent.None;

    private bool _suppressPropertyGetter;

    private string _invalidXExpr = string.Empty;

    private string _invalidYExpr = string.Empty;

    private string VectorXAsStr => IsPropertyHasErrors(nameof(XExpr)) ? _invalidXExpr : ((Vector2)Value).X.ToString(CultureInfo.InvariantCulture);

    private string VectorYAsStr => IsPropertyHasErrors(nameof(YExpr)) ? _invalidYExpr : ((Vector2)Value).Y.ToString(CultureInfo.InvariantCulture);

    #region XExpr

    public string XExpr
    {
        get => VectorXAsStr;
        set
        {
            ClearErrors();

            if (!Parser.TryParse(value, out var x, out var e))
            {
                _invalidXExpr = value;
                AddError($"Выражение '{value}' не может быть вычислено.\n{e!.Message}");
                OnPropertyChanged();
                return;
            }

            IsXValueUndefined = false;
            _suppressPropertyGetter = true;
            _changedVectorComponent = ChangedVectorComponent.X;
            Value = (Vector2)Value with { X = (float)x };
        }
    }

    #endregion

    #region IsXValueUndefined

    private bool _isXValueUndefined;

    public bool IsXValueUndefined
    {
        get => _isXValueUndefined;
        set => Set(ref _isXValueUndefined, value);
    }

    #endregion

    #region YExpr

    public string YExpr
    {
        get => VectorYAsStr;
        set
        {
            ClearErrors();

            if (!Parser.TryParse(value, out var y, out var e))
            {
                _invalidYExpr = value;
                AddError($"Выражение '{value}' не может быть вычислено.\n{e!.Message}");
                OnPropertyChanged();
                return;
            }

            IsYValueUndefined = false;
            _suppressPropertyGetter = true;
            _changedVectorComponent = ChangedVectorComponent.Y;
            Value = (Vector2)Value with { Y = (float)y };
        }
    }

    #endregion

    #region IsYValueUndefined

    private bool _isYValueUndefined;

    public bool IsYValueUndefined
    {
        get => _isYValueUndefined;
        set => Set(ref _isYValueUndefined, value);
    }

    #endregion

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        if (_suppressPropertyGetter)
            return Vector2.Zero;

        var firstObj = objects.First();

        IsXValueUndefined = objects.Any(o => !MathUtil.NearEqual(GetValue<Vector2>(o).X, GetValue<Vector2>(firstObj).X));
        IsYValueUndefined = objects.Any(o => !MathUtil.NearEqual(GetValue<Vector2>(o).Y, GetValue<Vector2>(firstObj).Y));

        return GetValue<Vector2>(firstObj);
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        var newVector = (Vector2)value;

        foreach (var obj in objects)
        {
            var oldVec = GetValue<Vector2>(obj);

            if (_changedVectorComponent == ChangedVectorComponent.X) oldVec.X = newVector.X;
            if (_changedVectorComponent == ChangedVectorComponent.Y) oldVec.Y = newVector.Y;

            SetValue(obj, oldVec);
        }

        _suppressPropertyGetter = false;
        _changedVectorComponent = ChangedVectorComponent.None;
    }
    
    protected override void OnEndEdit(IEnumerable<object> objects) => ClearAllErrors();

    protected override void OnPropertyChanged(string? propertyName = null)
    {
        if (propertyName == nameof(Value))
        {
            base.OnPropertyChanged(nameof(XExpr));
            base.OnPropertyChanged(nameof(YExpr));
        }
        else
        {
            base.OnPropertyChanged(propertyName);
        }
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new Vector2PropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };

    private enum ChangedVectorComponent
    {
        None,
        X,
        Y
    }
}