using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Views;

public class AndGateView : BaseGateView
{
    public float Scale => 1f;

    public override RectangleF Bounds => new(0, 0, 50 * Scale, 40 * Scale);

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var fillBrush = Cache.Get<SolidColorBrush>(this, FillBrushResource);
        var strokeBrush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);

        var strokeWidth = this.GetStrokeThickness(scene);

        var sink = context.ResourceFactory.BeginPathGeometry();
        sink.BeginFigure(new Vector2(Bounds.Width / 5f, Bounds.Height / 8f), FigureBegin.Filled);
        sink.AddLine(new Vector2(Bounds.Width / 2f, Bounds.Height / 8f));
        sink.AddArc(new ArcSegment { Point = new Vector2(Bounds.Width / 2f, Bounds.Height * 7f / 8f), ArcSize = ArcSize.Large, Size = new Size2F(Bounds.Width * 1.5f / 5f, Bounds.Width * 1.5f / 5f), RotationAngle = (float)Math.PI, SweepDirection = SweepDirection.Clockwise });
        sink.AddLine(new Vector2(Bounds.Width / 5f, Bounds.Height * 7f / 8f));
        sink.EndFigure(FigureEnd.Closed);
        using var path = context.ResourceFactory.EndPathGeometry();

        context.DrawingContext.FillGeometry(path, fillBrush);
        context.DrawingContext.DrawGeometry(path, strokeBrush, strokeWidth);

        context.DrawingContext.DrawLine(new Vector2(Bounds.Width * 4f / 5f, Bounds.Height / 2f), new Vector2(Bounds.Width, Bounds.Height / 2f), strokeBrush, strokeWidth);
        context.DrawingContext.DrawLine(new Vector2(0, Bounds.Height / 4f), new Vector2(Bounds.Width / 5f, Bounds.Height / 4f), strokeBrush, strokeWidth);
        context.DrawingContext.DrawLine(new Vector2(0, Bounds.Height * 3f / 4f), new Vector2(Bounds.Width / 5f, Bounds.Height * 3f / 4f), strokeBrush, strokeWidth);
    }
}