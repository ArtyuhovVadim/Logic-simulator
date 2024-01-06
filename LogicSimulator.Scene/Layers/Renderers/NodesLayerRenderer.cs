using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;
using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Layers.Renderers;

public class NodesLayerRenderer : BaseLayerRenderer<NodesLayer>
{
    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var strokeBrush = Layer.Cache.Get<SolidColorBrush>(this, NodesLayer.StrokeBrushResource);
        var fillBrush = Layer.Cache.Get<SolidColorBrush>(this, NodesLayer.FillBrushResource);

        var size = AbstractNode.NodeSize / scene.Scale;

        foreach (var sceneObject in Layer.Views.OfType<EditableSceneObjectView>().Where(x => x.IsSelected))
        {
            foreach (var node in sceneObject.Nodes)
            {
                var rect = node.GetLocation(sceneObject).RectangleRelativePointAsCenter(size);

                context.DrawingContext.FillRectangle(rect, fillBrush);
                context.DrawingContext.DrawRectangle(rect, strokeBrush, 1f / scene.Scale);
            }
        }
    }
}