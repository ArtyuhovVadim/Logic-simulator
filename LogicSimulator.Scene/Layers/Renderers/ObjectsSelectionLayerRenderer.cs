using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;

namespace LogicSimulator.Scene.Layers.Renderers;

public class ObjectsSelectionLayerRenderer : BaseLayerRenderer<ObjectsSelectionLayer>
{
    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        if (Layer?.Views is null)
            return;

        foreach (var obj in Layer.Views)
        {
            if (obj.IsSelected)
            {
                obj.RenderSelection(scene, context);
            }
        }
    }
}