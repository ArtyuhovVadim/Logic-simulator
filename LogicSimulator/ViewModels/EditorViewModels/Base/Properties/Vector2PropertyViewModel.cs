using System.ComponentModel;
using System.Globalization;
using LogicSimulator.Utils;
using MathExpressionParser;
using SharpDX;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class Vector2PropertyViewModel : SinglePropertyViewModel
{
    private Vector2ViewModel _vm = null!;
    private bool _suppressPropertyChanged;

    protected override void OnStartEdit(IEnumerable<object> objects)
    {
        _vm = new Vector2ViewModel(0, 0);
        _vm.PropertyChanged += OnVectorPropertyChanged;
    }

    protected override void OnEndEdit(IEnumerable<object> objects)
    {
        _vm.PropertyChanged -= OnVectorPropertyChanged;
    }

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        var firstObj = objects.First();

        if (!_vm.HasErrors)
        {
            _suppressPropertyChanged = true;
            var value = GetValue<Vector2>(firstObj);

            _vm.X = value.X.ToString(CultureInfo.InvariantCulture);
            _vm.Y = value.Y.ToString(CultureInfo.InvariantCulture);

            _vm.IsXUndefined = objects.Any(o => !MathUtil.NearEqual(GetValue<Vector2>(o).X, GetValue<Vector2>(firstObj).X));
            _vm.IsYUndefined = objects.Any(o => !MathUtil.NearEqual(GetValue<Vector2>(o).Y, GetValue<Vector2>(firstObj).Y));
            _suppressPropertyChanged = false;
        }

        return _vm;
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        var newVectorX = ((Vector2ViewModel)value).X;
        var newVectorY = ((Vector2ViewModel)value).Y;

        var newVectorXUndefined = ((Vector2ViewModel)value).IsXUndefined;
        var newVectorYUndefined = ((Vector2ViewModel)value).IsYUndefined;

        var newVectorXError = ((Vector2ViewModel)value).IsPropertyHasErrors(nameof(Vector2ViewModel.X));
        var newVectorYError = ((Vector2ViewModel)value).IsPropertyHasErrors(nameof(Vector2ViewModel.Y));

        foreach (var obj in objects)
        {
            var oldVec = GetValue<Vector2>(obj);
            if (!newVectorXUndefined && !newVectorXError) oldVec.X = float.Parse(newVectorX);
            if (!newVectorYUndefined && !newVectorYError) oldVec.Y = float.Parse(newVectorY);
            SetValue(obj, oldVec);
        }
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new Vector2PropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };

    private void OnVectorPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Vector2ViewModel.X) && e.PropertyName != nameof(Vector2ViewModel.Y)) return;

        if (_suppressPropertyChanged)
            return;

        Value = _vm;
    }

    private class Vector2ViewModel : ValidatableBindableBase
    {
        private static readonly MathParser Parser = MathParserBuilder.BuildDefaultParser();

        private string _invalidXValue = string.Empty;
        private string _invalidYValue = string.Empty;

        public Vector2ViewModel(float x, float y)
        {
            _x = x.ToString(CultureInfo.InvariantCulture);
            _y = y.ToString(CultureInfo.InvariantCulture);
        }

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

        #region X

        private string _x;

        public string X
        {
            get => _x;
            set
            {
                ClearErrors();

                if (!Parser.TryParse(value, out var x, out var e))
                {
                    _invalidXValue = value;
                    AddError($"Выражение '{value}' не может быть вычислено.\n{e!.Message}");
                    Set(ref _x, _invalidXValue);
                    return;
                }

                IsXUndefined = false;

                Set(ref _x, x.ToString(CultureInfo.InvariantCulture));
            }
        }

        #endregion

        #region Y

        private string _y;

        public string Y
        {
            get => _y;
            set
            {
                ClearErrors();

                if (!Parser.TryParse(value, out var y, out var e))
                {
                    _invalidYValue = value;
                    AddError($"Выражение '{value}' не может быть вычислено.\n{e!.Message}");
                    Set(ref _y, _invalidYValue);
                    return;
                }

                IsYUndefined = false;

                Set(ref _y, y.ToString(CultureInfo.InvariantCulture));
            }
        }

        #endregion
    }
}