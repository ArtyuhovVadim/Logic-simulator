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
    private float _lineThickness = 1f;
    private int _width = 100;
    private int _height = 100;
    private int _cellSize = 10;
    private int _boldLineStep = 10;

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

    public float LineThickness
    {
        get => _lineThickness;
        set => SetAndRequestRender(ref _lineThickness, value);
    }

    public int Width
    {
        get => _width;
        set => SetAndRequestRender(ref _width, value);
    }

    public int Height
    {
        get => _height;
        set => SetAndRequestRender(ref _height, value);
    }

    public int CellSize
    {
        get => _cellSize;
        set => SetAndRequestRender(ref _cellSize, value);
    }

    public int BoldLineStep
    {
        get => _boldLineStep;
        set => SetAndRequestRender(ref _boldLineStep, value);
    }

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

        for (var x = 1f; x <= (float)Width / CellSize; x++)
        {
            renderTarget.DrawLine(
                new Vector2(x * CellSize, 0f),
                new Vector2(x * CellSize, Height),
                x % BoldLineStep == 0 ? boldLineBrush : lineBrush,
                strokeWidth);
        }

        for (var y = 1f; y <= (float)Height / CellSize; y++)
        {
            renderTarget.DrawLine(
                new Vector2(0f, y * CellSize),
                new Vector2(Width, y * CellSize),
                y % BoldLineStep == 0 ? boldLineBrush : lineBrush,
                strokeWidth);
        }

        renderTarget.DrawRectangle(rect, boldLineBrush, strokeWidth);
    }
}