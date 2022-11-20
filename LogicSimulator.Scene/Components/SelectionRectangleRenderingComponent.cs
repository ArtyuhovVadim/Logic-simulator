using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class SelectionRectangleRenderingComponent : BaseRenderingComponent
{
    private static readonly Resource SecantBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((SelectionRectangleRenderingComponent)obj).SecantColor));

    private static readonly Resource NormalBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((SelectionRectangleRenderingComponent)obj).NormalColor));

    private Color4 _secantColor = new(0.39f, 0.78f, 0.39f, 1f);
    private Color4 _normalColor = new(0.49f, 0.68f, 1f, 1f);
    private Vector2 _startPosition;
    private Vector2 _endPosition;

    public SelectionRectangleRenderingComponent()
    {
        IsVisible = false;
    }

    public Vector2 StartPosition
    {
        get => _startPosition;
        set => SetAndRequestRender(ref _startPosition, value);
    }

    public Vector2 EndPosition
    {
        get => _endPosition;
        set => SetAndRequestRender(ref _endPosition, value);
    }

    public Color4 SecantColor
    {
        get => _secantColor;
        set => SetAndUpdateResource(ref _secantColor, value, SecantBrushResource);
    }

    public Color4 NormalColor
    {
        get => _normalColor;
        set => SetAndUpdateResource(ref _normalColor, value, NormalBrushResource);
    }

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(SecantBrushResource);
        InitializeResource(NormalBrushResource);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var brush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, EndPosition.X < StartPosition.X ? SecantBrushResource : NormalBrushResource, scene);

        var location = StartPosition;
        var size = EndPosition - StartPosition;

        renderTarget.DrawRectangle(new RectangleF { Location = location, Width = size.X, Height = size.Y }, brush, 1f / scene.Scale);
    }
}