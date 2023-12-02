using System.Windows.Media;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class LineViewModel : BaseObjectViewModel
{
    #region Vertexes

    private ObservableCollection<Vector2> _vertexes = new();
    
    public ObservableCollection<Vector2> Vertexes
    {
        get => _vertexes;
        set => Set(ref _vertexes, value);
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