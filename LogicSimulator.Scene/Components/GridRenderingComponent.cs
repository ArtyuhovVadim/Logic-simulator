using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class GridRenderingComponent : BaseRenderingComponent
{
    public static readonly Resource BackgroundBrushResource = ResourceCache.Register((target, o) =>
        new SolidColorBrush(target, ((GridRenderingComponent)o).Background));

    public static readonly Resource LineBrushResource = ResourceCache.Register((target, o) =>
        new SolidColorBrush(target, ((GridRenderingComponent)o).LineColor));

    public static readonly Resource BoldLineBrushResource = ResourceCache.Register((target, o) =>
        new SolidColorBrush(target, ((GridRenderingComponent)o).BoldLineColor));

    private Color4 _boldLineColor = Color4.Black;
    private Color4 _lineColor = Color4.Black;
    private Color4 _background = Color4.White;

    public Color4 Background
    {
        get => _background;
        set
        {
            ResourceCache.RequestUpdate(this, BackgroundBrushResource);
            _background = value;
        }
    }

    public Color4 LineColor
    {
        get => _lineColor;
        set
        {
            ResourceCache.RequestUpdate(this, LineBrushResource);
            _lineColor = value;
        }
    }

    public Color4 BoldLineColor
    {
        get => _boldLineColor;
        set
        {
            ResourceCache.RequestUpdate(this, BoldLineBrushResource);
            _boldLineColor = value;
        }
    }

    public float LineThickness { get; set; } = 1f;

    public int Width { get; set; } = 100;

    public int Height { get; set; } = 100;

    public int CellSize { get; set; } = 10;

    public int BoldLineStep { get; set; } = 10;

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeWidth = LineThickness / scene.Scale;
        var rect = new RectangleF(0, 0, Width, Height);

        var backgroundBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, BackgroundBrushResource, renderTarget);
        var lineBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, LineBrushResource, renderTarget);
        var boldLineBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, BoldLineBrushResource, renderTarget);

        renderTarget.FillRectangle(rect, backgroundBrush);

        renderTarget.DrawRectangle(rect, lineBrush, strokeWidth);

        for (var i = 0; i <= rect.Height / CellSize; i++)
        {
            renderTarget.DrawLine(
                new Vector2(i * CellSize + rect.Left, rect.Top),
                new Vector2(i * CellSize + rect.Left, rect.Bottom),
                i % BoldLineStep == 0 ? boldLineBrush : lineBrush,
                strokeWidth);
        }

        for (var i = 0; i <= rect.Width / CellSize; i++)
        {
            renderTarget.DrawLine(
                new Vector2(rect.Left, i * CellSize + rect.Top),
                new Vector2(rect.Right, i * CellSize + rect.Top),
                i % BoldLineStep == 0 ? boldLineBrush : lineBrush,
                strokeWidth);
        }
    }
}