using System.Windows.Media;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class ArcViewModel : BaseEditableObjectViewModel
{
    #region RadiusX

    private float _radiusX;

    public float RadiusX
    {
        get => _radiusX;
        set => Set(ref _radiusX, value);
    }

    #endregion

    #region RadiusY

    private float _radiusY;

    public float RadiusY
    {
        get => _radiusY;
        set => Set(ref _radiusY, value);
    }

    #endregion

    #region StartAngle

    private float _startAngle;

    public float StartAngle
    {
        get => _startAngle;
        set => Set(ref _startAngle, value);
    }

    #endregion

    #region EndAngle

    private float _endAngle = 180f;

    public float EndAngle
    {
        get => _endAngle;
        set => Set(ref _endAngle, value);
    }

    #endregion

    #region StrokeColor

    private Color _strokeColor = Colors.Black;

    public Color StrokeColor
    {
        get => _strokeColor;
        set => Set(ref _strokeColor, value);
    }

    #endregion

    #region StrokeThickness

    private float _strokeThickness = 1f;

    public float StrokeThickness
    {
        get => _strokeThickness;
        set => Set(ref _strokeThickness, value);
    }

    #endregion
}