using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Layers.Renderers;

public class GridLayerRenderer : BaseLayerRenderer<GridLayer>
{
    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var strokeWidth = Layer.LineThickness / scene.Scale;
        var rect = new RectangleF(0, 0, Layer.Width, Layer.Height);

        var backgroundBrush = Layer.Cache.Get<SolidColorBrush>(this, GridLayer.BackgroundBrushResource);
        var lineBrush = Layer.Cache.Get<SolidColorBrush>(this, GridLayer.LineBrushResource);
        var boldLineBrush = Layer.Cache.Get<SolidColorBrush>(this, GridLayer.BoldLineBrushResource);

        context.DrawingContext.FillRectangle(rect, backgroundBrush);

        for (var x = 1f; x <= (float)Layer.Width / Layer.CellSize; x++)
        {
            context.DrawingContext.DrawLine(
                new Vector2(x * Layer.CellSize, 0f),
                new Vector2(x * Layer.CellSize, Layer.Height),
                x % Layer.BoldLineStep == 0 ? boldLineBrush : lineBrush,
                strokeWidth);
        }

        for (var y = 1f; y <= (float)Layer.Height / Layer.CellSize; y++)
        {
            context.DrawingContext.DrawLine(
                new Vector2(0f, y * Layer.CellSize),
                new Vector2(Layer.Width, y * Layer.CellSize),
                y % Layer.BoldLineStep == 0 ? boldLineBrush : lineBrush,
                strokeWidth);
        }

        context.DrawingContext.DrawRectangle(rect, boldLineBrush, strokeWidth);
    }
}