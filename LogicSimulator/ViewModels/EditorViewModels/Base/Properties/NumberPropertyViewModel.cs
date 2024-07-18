using System.Globalization;
using System.Numerics;
using LogicSimulator.Utils;

namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class NumberPropertyViewModel<T> : BaseNumberPropertyViewModel where T : INumber<T>, IMinMaxValue<T>
{
    private string _invalidValue = string.Empty;

    #region IsNanAllowed

    private bool _isNanAllowed;

    public bool IsNanAllowed
    {
        get => _isNanAllowed;
        set => Set(ref _isNanAllowed, value);
    }

    #endregion

    #region MaxNumber

    private T _maxNumber = T.MaxValue;

    public T MaxNumber
    {
        get => _maxNumber;
        set => Set(ref _maxNumber, value);
    }

    #endregion

    #region MinNumber

    private T _minNumber = T.MinValue;

    public T MinNumber
    {
        get => _minNumber;
        set => Set(ref _minNumber, value);
    }

    #endregion

    #region NumberSuffix

    private string _numberSuffix = string.Empty;

    public string NumberSuffix
    {
        get => _numberSuffix;
        set => Set(ref _numberSuffix, value);
    }

    #endregion

    #region DisplayCoefficient

    private T _displayCoefficient = T.One;

    public T DisplayCoefficient
    {
        get => _displayCoefficient;
        set => Set(ref _displayCoefficient, value);
    }

    #endregion

    protected override object GetPropertyValue(IReadOnlyCollection<object> objects)
    {
        if (HasErrors)
            return _invalidValue;

        var firstObj = objects.First();
        var firstObjValue = GetValue<T>(firstObj);

        IsValueUndefined = objects.Any(o =>
        {
            if (T.IsNaN(GetValue<T>(o)) ^ T.IsNaN(GetValue<T>(firstObj)))
                return true;

            if (T.IsNaN(GetValue<T>(o)) && T.IsNaN(GetValue<T>(firstObj)))
                return false;

            return GetValue<T>(o) != GetValue<T>(firstObj);
        });

        if (NumberSuffix.Length != 0 && !T.IsNaN(firstObjValue))
            return string.Format(CultureInfo.InvariantCulture, "{0:0.###}{1}", firstObjValue / DisplayCoefficient, NumberSuffix);

        return GetValue<T>(firstObj);
    }

    protected override void SetPropertyValue(IReadOnlyCollection<object> objects, object value)
    {
        ClearAllErrors();

        var originalExpr = (string)value;
        var exprWithoutSuffix = originalExpr;

        if (NumberSuffix.Length != 0)
        {
            exprWithoutSuffix = originalExpr.Replace(NumberSuffix, string.Empty);
        }

        if (!Parser.TryParse(exprWithoutSuffix, out var number, out var e))
        {
            _invalidValue = originalExpr;
            AddError($"Выражение '{originalExpr}' не может быть вычислено.\n{e!.Message}", nameof(Value));
            return;
        }

        var numberT = T.CreateSaturating(number) * DisplayCoefficient;

        if (!IsNanAllowed && T.IsNaN(numberT))
        {
            _invalidValue = originalExpr;
            AddError("NaN не разрешен", nameof(Value));
            return;
        }

        if (numberT > MaxNumber || numberT < MinNumber)
        {
            _invalidValue = originalExpr;
            AddError($"Число должно находиться в интервале [{MinNumber}, {MaxNumber}]", nameof(Value));
            return;
        }

        IsValueUndefined = false;

        foreach (var obj in objects)
        {
            SetValue(obj, numberT);
        }
    }

    protected override void OnEndEdit(IEnumerable<object> objects) => ClearAllErrors();

    public override PropertyViewModel MakeCopy(EditorViewModel editor) => new NumberPropertyViewModel<T>
    {
        PropertyName = PropertyName,
        EditorViewModel = editor,
        MinNumber = MinNumber,
        MaxNumber = MaxNumber,
        DisplayCoefficient = DisplayCoefficient,
        NumberSuffix = NumberSuffix,
        IsNanAllowed = IsNanAllowed
    };
}