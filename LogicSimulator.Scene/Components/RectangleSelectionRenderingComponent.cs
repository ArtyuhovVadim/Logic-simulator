using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class RectangleSelectionRenderingComponent : BaseRenderingComponent
{
    public static readonly Resource SecantBrushResource =
        Resource.Register<RectangleSelectionRenderingComponent, SolidColorBrush>(nameof(SecantBrushResource),
            (target, o) => new SolidColorBrush(target, ((RectangleSelectionRenderingComponent) o).SecantColor));

    public static readonly Resource NormalBrushResource =
        Resource.Register<RectangleSelectionRenderingComponent, SolidColorBrush>(nameof(NormalBrushResource),
            (target, o) => new SolidColorBrush(target, ((RectangleSelectionRenderingComponent) o).NormalColor));

    private Vector2 _startPosition;
    private Vector2 _endPosition;

    private Color4 _secantColor = new(0.39f, 0.78f, 0.39f, 1f);
    private Color4 _normalColor = new(0.49f, 0.68f, 1f, 1f);

    public RectangleSelectionRenderingComponent()
    {
        IsVisible = false;
    }

    public Vector2 StartPosition
    {
        get => _startPosition;
        set
        {
            _startPosition = value;
            RequireRender();
        }
    }

    public Vector2 EndPosition
    {
        get => _endPosition;
        set
        {
            _endPosition = value;
            RequireRender();
        }
    }

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

    public override void Render(Scene2D scene, Renderer renderer)
    {
        if (IsVisible)
        {
            renderer.Render(scene, this);
        }
    }
}