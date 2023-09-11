using System.Windows.Media;
using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class EllipseViewModel : BaseEditableObjectViewModel
{
    #region RadiusX

    private float _radiusX;
    
    [Editable]
    public float RadiusX
    {
        get => _radiusX;
        set => Set(ref _radiusX, value);
    }

    #endregion

    #region RadiusY

    private float _radiusY;
    
    [Editable]
    public float RadiusY
    {
        get => _radiusY;
        set => Set(ref _radiusY, value);
    }

    #endregion

    #region FillColor

    private Color _fillColor = Colors.White;
    
    [Editable]
    public Color FillColor
    {
        get => _fillColor;
        set => Set(ref _fillColor, value);
    }

    #endregion

    #region StrokeColor

    private Color _strokeColor = Colors.Black;
    
    [Editable]
    public Color StrokeColor
    {
        get => _strokeColor;
        set => Set(ref _strokeColor, value);
    }

    #endregion

    #region StrokeThickness

    private float _strokeThickness = 1f;
    
    [Editable]
    public float StrokeThickness
    {
        get => _strokeThickness;
        set => Set(ref _strokeThickness, value);
    }

    #endregion

    #region IsFilled

    private bool _isFilled = true;
    
    [Editable]
    public bool IsFilled
    {
        get => _isFilled;
        set => Set(ref _isFilled, value);
    }

    #endregion
}