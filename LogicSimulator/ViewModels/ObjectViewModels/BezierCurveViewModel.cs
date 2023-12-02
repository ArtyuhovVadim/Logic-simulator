using System.Windows.Media;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class BezierCurveViewModel : BaseObjectViewModel
{
    #region Point1

    private Vector2 _point1;
    
    public Vector2 Point1
    {
        get => _point1;
        set => Set(ref _point1, value);
    }

    #endregion

    #region Point2

    private Vector2 _point2;
    
    public Vector2 Point2
    {
        get => _point2;
        set => Set(ref _point2, value);
    }

    #endregion

    #region Point3

    private Vector2 _point3;
    
    public Vector2 Point3
    {
        get => _point3;
        set => Set(ref _point3, value);
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