using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class GridRenderingComponent : BaseRenderingComponent
{
    private static readonly Resource BackgroundBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((GridRenderingComponent)obj).Background));

    private static readonly Resource LineBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((GridRenderingComponent)obj).LineColor));

    private static readonly Resource BoldLineBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((GridRenderingComponent)obj).BoldLineColor));

    private Color4 _boldLineColor = Color4.Black;
    private Color4 _lineColor = Color4.Black;
    private Color4 _background = Color4.White;

    public Color4 Background
    {
        get => _background;
        set => SetAndUpdateResource(ref _background, value, BackgroundBrushResource);
    }

    public Color4 LineColor
    {
        get => _lineColor;
        set => SetAndUpdateResource(ref _lineColor, value, LineBrushResource);
    }

    public Color4 BoldLineColor
    {
        get => _boldLineColor;
        set => SetAndUpdateResource(ref _boldLineColor, value, BoldLineBrushResource);
    }

    public float LineThickness { get; set; } = 1f;

    public int Width { get; set; } = 100;

    public int Height { get; set; } = 100;

    public int CellSize { get; set; } = 10;

    public int BoldLineStep { get; set; } = 10;

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(BackgroundBrushResource);
        InitializeResource(LineBrushResource);
        InitializeResource(BoldLineBrushResource);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeWidth = LineThickness / scene.Scale;
        var rect = new RectangleF(0, 0, Width, Height);

        var backgroundBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, BackgroundBrushResource, scene);
        var lineBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, LineBrushResource, scene);
        var boldLineBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, BoldLineBrushResource, scene);

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