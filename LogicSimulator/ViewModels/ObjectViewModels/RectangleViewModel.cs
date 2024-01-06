using System.Windows.Media;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class RectangleViewModel : BaseObjectViewModel
{
    #region Width

    private float _width;

    public float Width
    {
        get => _width;
        set => Set(ref _width, value);
    }

    #endregion

    #region Height

    private float _height;
    
    public float Height
    {
        get => _height;
        set => Set(ref _height, value);
    }

    #endregion

    #region FillColor

    private Color _fillColor = Colors.White;
    
    public Color FillColor
    {
        get => _fillColor;
        set => Set(ref _fillColor, value);
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

    #region StrokeThicknessType

    private StrokeThicknessType _strokeThicknessType = StrokeThicknessType.Smallest;

    public StrokeThicknessType StrokeThicknessType
    {
        get => _strokeThicknessType;
        set => Set(ref _strokeThicknessType, value);
    }

    #endregion

    #region IsFilled

    private bool _isFilled = true;
    
    public bool IsFilled
    {
        get => _isFilled;
        set => Set(ref _isFilled, value);
    }

    #endregion
}