using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Views;

public class AndGateView : BaseGateView
{
    public override RectangleF SelectionRect
    {
        get
        {
            var path = Cache.Get<PathGeometry>(AndGeometryResource);
            var bounds = path.GetBounds().ToRect();
            bounds.Inflate(bounds.X, bounds.Y);
            return bounds.ScaleSize(Scale);
        }
    }

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var fillBrush = Cache.Get<SolidColorBrush>(this, FillBrushResource);
        var strokeBrush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);
        var strokeWidth = this.GetStrokeThickness(scene) / Scale;
        var path = Cache.Get<PathGeometry>(AndGeometryResource);

        context.DrawingContext.PushTransform(Matrix3x2.Scaling(Scale, Scale));
        context.DrawingContext.PushAntialiasMode(AntialiasMode.PerPrimitive);
        context.DrawingContext.FillGeometry(path, fillBrush);
        context.DrawingContext.DrawGeometry(path, strokeBrush, strokeWidth);
        context.DrawingContext.PopAntialiasMode();
        context.DrawingContext.PopTransform();
    }
}