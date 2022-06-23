using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class GridRenderingComponent : BaseRenderingComponent
{
    private Color4 _boldLineColor = Color4.Black;
    private Color4 _lineColor = Color4.Black;
    private Color4 _background = Color4.White;

    public Color4 Background
    {
        get => _background;
        set
        {
            RequireUpdate(BackgroundResource);
            _background = value;
        }
    }

    public Color4 LineColor
    {
        get => _lineColor;
        set
        {
            RequireUpdate(LineResource);
            _lineColor = value;
        }
    }

    public Color4 BoldLineColor
    {
        get => _boldLineColor;
        set
        {
            RequireUpdate(BoldLineResource);
            _boldLineColor = value;
        }
    }

    public float LineThickness { get; set; } = 1f;

    public int Width { get; set; } = 100;

    public int Height { get; set; } = 100;

    public int CellSize { get; set; } = 10;

    public int BoldLineStep { get; set; } = 10;

    public override void Render(ComponentRenderer renderer)
    {
        renderer.Render(this);
    }

    public static readonly Resource BackgroundResource = Resource.Register<GridRenderingComponent, SolidColorBrush>(nameof(BackgroundResource),
        (target, o) => new SolidColorBrush(target, ((GridRenderingComponent)o).Background));

    public static readonly Resource LineResource = Resource.Register<GridRenderingComponent, SolidColorBrush>(nameof(LineResource),
        (target, o) => new SolidColorBrush(target, ((GridRenderingComponent)o).LineColor));

    public static readonly Resource BoldLineResource = Resource.Register<GridRenderingComponent, SolidColorBrush>(nameof(BoldLineResource),
        (target, o) => new SolidColorBrush(target, ((GridRenderingComponent)o).BoldLineColor));
}