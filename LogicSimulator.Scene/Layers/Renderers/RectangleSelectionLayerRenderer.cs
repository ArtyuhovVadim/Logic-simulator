using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Layers.Renderers;

public class RectangleSelectionLayerRenderer : BaseLayerRenderer<RectangleSelectionLayer>
{
    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var brush = Layer.Cache.Get<SolidColorBrush>(this,
            Layer.EndPosition.X < Layer.StartPosition.X ? RectangleSelectionLayer.SecantBrushResource : RectangleSelectionLayer.NormalBrushResource);

        var location = Layer.StartPosition;
        var size = Layer.EndPosition - Layer.StartPosition;

        context.DrawingContext.DrawRectangle(new RectangleF { Location = location, Width = size.X, Height = size.Y }, brush, 1f / scene.Scale);
    }
}