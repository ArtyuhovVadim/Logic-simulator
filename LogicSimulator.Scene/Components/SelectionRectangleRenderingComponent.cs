using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class SelectionRectangleRenderingComponent : BaseRenderingComponent
{
    public static readonly Resource SecantBrushResource =
        Resource.Register<SelectionRectangleRenderingComponent, SolidColorBrush>(nameof(SecantBrushResource),
            (target, o) => new SolidColorBrush(target, ((SelectionRectangleRenderingComponent)o).SecantColor));

    public static readonly Resource NormalBrushResource =
        Resource.Register<SelectionRectangleRenderingComponent, SolidColorBrush>(nameof(NormalBrushResource),
            (target, o) => new SolidColorBrush(target, ((SelectionRectangleRenderingComponent)o).NormalColor));

    private Color4 _secantColor = new(0.39f, 0.78f, 0.39f, 1f);
    private Color4 _normalColor = new(0.49f, 0.68f, 1f, 1f);

    public SelectionRectangleRenderingComponent()
    {
        IsVisible = false;
    }

    public Vector2 StartPosition { get; set; }

    public Vector2 EndPosition { get; set; }

    public Color4 SecantColor
    {
        get => _secantColor;
        set
        {
            _secantColor = value;
            RequireUpdate(SecantBrushResource);
        }
    }

    public Color4 NormalColor
    {
        get => _normalColor;
        set
        {
            _normalColor = value;
            RequireUpdate(NormalBrushResource);
        }
    }

    public bool IsSecant { get; set; }

    public override void Render(Scene2D scene, RenderTarget renderTarget)
    {
        if (!IsVisible) return;

        var brush = GetResourceValue<SolidColorBrush>(IsSecant ? SecantBrushResource : NormalBrushResource, renderTarget);

        var location = StartPosition;
        var size = EndPosition - StartPosition;

        renderTarget.DrawRectangle(new RectangleF { Location = location, Width = size.X, Height = size.Y }, brush, 1f / scene.Scale);
    }
}