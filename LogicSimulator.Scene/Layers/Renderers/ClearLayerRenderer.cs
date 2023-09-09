using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;
using LogicSimulator.Utils;

namespace LogicSimulator.Scene.Layers.Renderers;

public class ClearLayerRenderer : BaseLayerRenderer<ClearLayer>
{
    protected override void OnRender(Scene2D scene, D2DContext context) => context.DrawingContext.Clear(Layer.Color.ToColor4());
}