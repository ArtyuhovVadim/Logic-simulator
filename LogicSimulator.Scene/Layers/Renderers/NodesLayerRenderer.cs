using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Shared;
using LogicSimulator.Utils;

namespace LogicSimulator.Scene.Layers.Renderers;

public class NodesLayerRenderer : BaseLayerRenderer<NodesLayer>
{
    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var strokeBrush = Layer.Cache.Get(this, NodesLayer.StrokeBrushResource);
        var fillBrush = Layer.Cache.Get(this, NodesLayer.FillBrushResource);

        var size = IEditableObjectNode.NodeSize / scene.Scale;

        foreach (var sceneObject in Layer.Views.OfType<EditableSceneObjectView>().Where(x => x is { IsSelected: true, IsDragging: false }))
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