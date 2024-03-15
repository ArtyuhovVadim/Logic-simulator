using LogicSimulator.Scene;
using LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;
using System.Windows.Media;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates;

public class AndGateViewModel : BaseGateViewModel
{
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

    public override AndGateViewModel MakeClone() => (AndGateViewModel)MemberwiseClone();
}