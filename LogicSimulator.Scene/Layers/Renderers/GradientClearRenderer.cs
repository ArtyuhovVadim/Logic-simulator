using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;
using SharpDX;

namespace LogicSimulator.Scene.Layers.Renderers;

public class GradientClearRenderer : BaseLayerRenderer<GradientClearLayer>
{
    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var width = scene.PixelSize.Width;
        var height = scene.PixelSize.Height;
        
        var brush = Layer.Cache.Get(this, GradientClearLayer.BrushResource);

        context.DrawingContext.PushTransform(Matrix3x2.Invert(scene.Transform));
        context.DrawingContext.FillRectangle(new RectangleF(0, 0, width, height), brush);
        context.DrawingContext.PopTransform();
    }
}