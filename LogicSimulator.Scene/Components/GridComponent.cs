using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class GridComponent : BaseComponent
{
    public static readonly Resource BackgroundBrushResource = Resource.Register<GridComponent, SolidColorBrush>(nameof(BackgroundBrushResource),
        (target, o) => new SolidColorBrush(target, ((GridComponent)o).Background));

    public static readonly Resource LineBrushResource = Resource.Register<GridComponent, SolidColorBrush>(nameof(LineBrushResource),
        (target, o) => new SolidColorBrush(target, ((GridComponent)o).LineColor));

    public static readonly Resource BoldLineBrushResource = Resource.Register<GridComponent, SolidColorBrush>(nameof(BoldLineBrushResource),
        (target, o) => new SolidColorBrush(target, ((GridComponent)o).BoldLineColor));

    private Color4 _boldLineColor = Color4.Black;
    private Color4 _lineColor = Color4.Black;
    private Color4 _background = Color4.White;

    public Color4 Background
    {
        get => _background;
        set
        {
            RequireUpdate(BackgroundBrushResource);
            _background = value;
        }
    }

    public Color4 LineColor
    {
        get => _lineColor;
        set
        {
            RequireUpdate(LineBrushResource);
            _lineColor = value;
        }
    }

    public Color4 BoldLineColor
    {
        get => _boldLineColor;
        set
        {
            RequireUpdate(BoldLineBrushResource);
            _boldLineColor = value;
        }
    }

    public float LineThickness { get; set; } = 1f;

    public int Width { get; set; } = 100;

    public int Height { get; set; } = 100;

    public int CellSize { get; set; } = 10;

    public int BoldLineStep { get; set; } = 10;

    public override void Render(Renderer renderer) => renderer.Render(this);
}