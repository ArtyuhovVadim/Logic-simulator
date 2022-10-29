using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class SelectionRenderingComponent : BaseRenderingComponent
{
    private static readonly Resource SelectionBrushResource = ResourceCache.Register((target, o) =>
        new SolidColorBrush(target, ((SelectionRenderingComponent)o).SelectionColor));

    private static readonly Resource SelectionStyleResource = ResourceCache.Register((target, _) =>
    {
        var properties = new StrokeStyleProperties
        {
            DashStyle = DashStyle.Custom,
            DashCap = CapStyle.Flat
        };

        return new StrokeStyle(target.Factory, properties, new[] { 2f, 2f });
    });

    private Color4 _selectionColor = new(0, 1, 0, 1);

    public Color4 SelectionColor
    {
        get => _selectionColor;
        set => SetAndUpdateResource(ref _selectionColor, value, SelectionBrushResource);
    }

    protected override void OnInitialize(Scene2D scene, RenderTarget renderTarget)
    {
        InitializeResource(SelectionBrushResource);
        InitializeResource(SelectionStyleResource);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var brush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, SelectionBrushResource, renderTarget);
        var style = ResourceCache.GetOrUpdate<StrokeStyle>(this, SelectionStyleResource, renderTarget);

        foreach (var sceneObject in scene.Objects)
        {
            if (sceneObject.IsSelected)
            {
                sceneObject.RenderSelection(scene, renderTarget, brush, style);
            }
        }
    }
}