using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class GridRenderingComponent : BaseRenderingComponent
{
    public static readonly Resource BackgroundBrushResource = Resource.Register<GridRenderingComponent, SolidColorBrush>(nameof(BackgroundBrushResource),
        (target, o) => new SolidColorBrush(target, ((GridRenderingComponent)o).Background));

    public static readonly Resource LineBrushResource = Resource.Register<GridRenderingComponent, SolidColorBrush>(nameof(LineBrushResource),
        (target, o) => new SolidColorBrush(target, ((GridRenderingComponent)o).LineColor));

    public static readonly Resource BoldLineBrushResource = Resource.Register<GridRenderingComponent, SolidColorBrush>(nameof(BoldLineBrushResource),
        (target, o) => new SolidColorBrush(target, ((GridRenderingComponent)o).BoldLineColor));

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

    public override void Render(Scene2D scene, Renderer renderer)
    {
        if (IsVisible)
        {
            renderer.Render(scene, this);
        }
    }
}