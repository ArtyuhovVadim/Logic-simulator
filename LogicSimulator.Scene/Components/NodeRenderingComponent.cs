using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class NodeRenderingComponent : BaseRenderingComponent
{
    public static readonly Resource StrokeBrushResource = Resource.Register<NodeRenderingComponent, SolidColorBrush>(nameof(StrokeBrushResource),
        (target, o) => new SolidColorBrush(target, Color4.Black));

    public static readonly Resource SelectBrushResource = Resource.Register<NodeRenderingComponent, SolidColorBrush>(nameof(SelectBrushResource),
        (target, o) => new SolidColorBrush(target, new Color4(1, 0, 0, 1)));

    public static readonly Resource UnselectBrushResource = Resource.Register<NodeRenderingComponent, SolidColorBrush>(nameof(UnselectBrushResource),
        (target, o) => new SolidColorBrush(target, new Color4(0, 1, 0, 1)));

    public override void Render(Scene2D scene, Renderer renderer) => renderer.Render(scene, this);
}