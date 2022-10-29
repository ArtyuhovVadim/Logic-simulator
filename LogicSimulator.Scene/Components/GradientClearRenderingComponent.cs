using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class GradientClearRenderingComponent : BaseRenderingComponent
{
    private static readonly Resource ClearGradientBrushResource = ResourceCache.Register((target, o) =>
    {
        var component = (GradientClearRenderingComponent)o;

        var gradientStopCollection = new GradientStopCollection(target, new GradientStop[]
        {
            new() { Position = 0f, Color = component.StartColor },
            new() { Position = 1f, Color = component.EndColor }
        });
        
        var properties = new LinearGradientBrushProperties
        {
            StartPoint = new Vector2(target.Size.Width / 2, 0),
            EndPoint = new Vector2(target.Size.Width / 2, target.Size.Height)
        };

        var brush = new LinearGradientBrush(target, properties, gradientStopCollection);

        Utilities.Dispose(ref gradientStopCollection);

        return brush;
    });

    private Color4 _startColor = Color4.White;
    private Color4 _endColor = Color4.Black;

    public Color4 StartColor
    {
        get => _startColor;
        set => SetAndUpdateResource(ref _startColor, value, ClearGradientBrushResource);
    }

    public Color4 EndColor
    {
        get => _endColor;
        set => SetAndUpdateResource(ref _endColor, value, ClearGradientBrushResource);
    }

    protected override void OnInitialize(Scene2D scene, RenderTarget renderTarget)
    {
        InitializeResource(ClearGradientBrushResource);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var tempTransform = renderTarget.Transform;
        renderTarget.Transform = Matrix3x2.Identity;

        var brush = ResourceCache.GetOrUpdate<LinearGradientBrush>(this, ClearGradientBrushResource, renderTarget);

        renderTarget.FillRectangle(new RectangleF(0, 0, renderTarget.Size.Width, renderTarget.Size.Height), brush);

        renderTarget.Transform = tempTransform;
    }
}