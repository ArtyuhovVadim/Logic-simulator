using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Scene.Layers.Renderers;

public class ClearLayerRenderer : BaseLayerRenderer<ClearLayer>
{
    protected override void OnRender(Scene2D scene, D2DContext context) => context.DrawingContext.Clear(ToColor4(Layer.Color));

    //TODO: Move
    private static Color4 ToColor4(Color color)
    {
        var a = color.A / 255f;
        var r = color.R / 255f;
        var g = color.G / 255f;
        var b = color.B / 255f;

        return new Color4(r, g, b, a);
    }
}