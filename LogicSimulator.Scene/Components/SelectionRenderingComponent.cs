using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class SelectionRenderingComponent : BaseRenderingComponent
{
    public static readonly Resource SelectionBrushResource = Resource.Register<SelectionRenderingComponent, SolidColorBrush>(nameof(SelectionBrushResource),
        (target, o) => new SolidColorBrush(target, ((SelectionRenderingComponent)o).SelectionColor));

    public static readonly Resource SelectionStyleResource = Resource.Register<SelectionRenderingComponent, StrokeStyle>(nameof(SelectionStyleResource),
        (target, _) =>
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
        set
        {
            _selectionColor = value;
            RequireUpdate(SelectionBrushResource);
        }
    }

    public override void Render(Scene2D scene, RenderTarget renderTarget)
    {
        if (!IsVisible) return;

        var brush = GetResourceValue<SolidColorBrush>(SelectionBrushResource, renderTarget);
        var style = GetResourceValue<StrokeStyle>(SelectionStyleResource, renderTarget);

        foreach (var sceneObject in scene.Objects)
        {
            if (sceneObject.IsSelected)
            {
                sceneObject.RenderSelection(scene, renderTarget, brush, style);
            }
        }
    }
}