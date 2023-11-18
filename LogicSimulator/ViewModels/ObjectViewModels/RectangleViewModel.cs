using System.Windows.Media;
using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class RectangleViewModel : BaseObjectViewModel
{
    #region Width

    private float _width;

    [Editable]
    public float Width
    {
        get => _width;
        set => Set(ref _width, value);
    }

    #endregion

    #region Height

    private float _height;
    
    [Editable]
    public float Height
    {
        get => _height;
        set => Set(ref _height, value);
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