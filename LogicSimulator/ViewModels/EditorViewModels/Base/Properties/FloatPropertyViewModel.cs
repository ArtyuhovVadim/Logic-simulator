using System.Globalization;
using LogicSimulator.Utils;
using MathExpressionParser;

namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class FloatPropertyViewModel : SinglePropertyViewModel
{
    private static readonly MathParser Parser = MathParserBuilder.BuildDefaultParser();
    private string _invalidValue = string.Empty;

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

    #region NumberSuffix

    private string _numberSuffix = string.Empty;

    public string NumberSuffix
    {
        get => _numberSuffix;
        set => Set(ref _numberSuffix, value);
    }

    #endregion

    #region DisplayCoefficient

    private float _displayCoefficient = 1f;

    public float DisplayCoefficient
    {
        get => _displayCoefficient;
        set => Set(ref _displayCoefficient, value);
    }

    #endregion

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        if (HasErrors)
            return _invalidValue;

        var firstObj = objects.First();

        IsValueUndefined = objects.Any(o => !Equals(GetValue<float>(o), GetValue<float>(firstObj)));

        if (NumberSuffix.Length != 0)
            return string.Format(CultureInfo.InvariantCulture, "{0:0.###}{1}", GetValue<float>(firstObj) / DisplayCoefficient, NumberSuffix);

        return GetValue<float>(firstObj);
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        ClearAllErrors();

        var expr = (string)value;

        if (NumberSuffix.Length != 0 && expr.EndsWith(NumberSuffix))
        {
            expr = expr[..^NumberSuffix.Length];
        }

        if (!Parser.TryParse(expr, out var number, out var e))
        {
            _invalidValue = expr;
            AddError($"Выражение '{expr}' не может быть вычислено.\n{e!.Message}", nameof(Value));
            return;
        }

        if (number > MaxNumber || number < MinNumber)
        {
            _invalidValue = expr;
            AddError($"Число должно находиться в интервале [{MinNumber}, {MaxNumber}]", nameof(Value));
            return;
        }

        IsValueUndefined = false;

        var newValue = (float)number * DisplayCoefficient;

        foreach (var obj in objects)
        {
            SetValue(obj, newValue);
        }
    }

    protected override void OnEndEdit(IEnumerable<object> objects) => ClearAllErrors();

    public override PropertyViewModel MakeCopy(EditorViewModel editor) => new FloatPropertyViewModel
    {
        PropertyName = PropertyName,
        EditorViewModel = editor,
        MinNumber = MinNumber,
        MaxNumber = MaxNumber,
        DisplayCoefficient = DisplayCoefficient,
        NumberSuffix = NumberSuffix
    };
}