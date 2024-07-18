using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;
using LogicSimulator.Utils;

namespace LogicSimulator.Scene.Layers.Renderers;

public class ObjectsLayerRenderer : BaseLayerRenderer<ObjectsLayer>
{
    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var viewport = scene.ViewportInWorldSpace.ToInflated(32);

        foreach (var view in Layer.Views.Where(x => x.WorldBounds.IntersectsInclusive(viewport)))
        {
            view.Render(scene, context);
        }
    }
}