using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;

namespace LogicSimulator.Scene.Layers.Renderers;

public class ObjectsLayerRenderer : BaseLayerRenderer<ObjectsLayer>
{
    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        foreach (var view in Layer.Views)
        { 
            view.Render(scene, context);
        }
    }
}