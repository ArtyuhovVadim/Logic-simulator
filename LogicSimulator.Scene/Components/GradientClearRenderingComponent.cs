using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class GradientClearRenderingComponent : BaseRenderingComponent
{
    private static readonly Resource ClearGradientBrushResource = ResourceCache.Register((scene, obj) =>
    {
        var component = (GradientClearRenderingComponent)obj;

        var gradientStopCollection = new GradientStop[]
        {
            new() { Position = 0f, Color = component.StartColor },
            new() { Position = 1f, Color = component.EndColor }
        };

        var properties = new LinearGradientBrushProperties
        {
            StartPoint = new Vector2(scene.Size.X / 2, 0),
            EndPoint = new Vector2(scene.Size.X / 2, scene.Size.Y)
        };

        return scene.ResourceFactory.CreateLinearGradientBrush(properties, gradientStopCollection);
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

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(ClearGradientBrushResource);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var tempTransform = renderTarget.Transform;
        renderTarget.Transform = Matrix3x2.Identity;

        var brush = ResourceCache.GetOrUpdate<LinearGradientBrush>(this, ClearGradientBrushResource, scene);

        renderTarget.FillRectangle(new RectangleF(0, 0, renderTarget.Size.Width, renderTarget.Size.Height), brush);

        renderTarget.Transform = tempTransform;
    }
}