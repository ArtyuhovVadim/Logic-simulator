using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace LogicSimulator.Scene.Views;

public class UnknownObjectView : SceneObjectView
{
    public override bool HitTest(Vector2 pos, Matrix3x2 worldTransform, float tolerance = 0.25f) => false;

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 worldTransform, float tolerance = 0.25f) => GeometryRelation.Unknown;

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        using var blackBrush = context.ResourceFactory.CreateSolidColorBrush(Color.Black);
        using var whiteBrush = context.ResourceFactory.CreateSolidColorBrush(Color.White);
        using var redBrush = context.ResourceFactory.CreateSolidColorBrush(Color.Red);

        var typeName = DataContext.GetType().Name;

        using var textFormat = context.ResourceFactory.CreateTextFormat("Calibry", 16f);
        using var textLayout = context.ResourceFactory.CreateTextLayout($"Unknown object\n{typeName}", textFormat);

        var metrics = textLayout.Metrics;
        var margin = 3;

        textLayout.TextAlignment = TextAlignment.Center;
        textLayout.MaxWidth = metrics.Width;
        textLayout.MaxHeight = metrics.Height;

        context.DrawingContext.FillRectangle(new RectangleF(-margin, -margin, metrics.Width + margin * 2, metrics.Height + margin * 2), whiteBrush);
        context.DrawingContext.DrawRectangle(new RectangleF(-margin, -margin, metrics.Width + margin * 2, metrics.Height + margin * 2), blackBrush, 1f);
        context.DrawingContext.DrawTextLayout(Vector2.Zero, textLayout, redBrush, DrawTextOptions.None);
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        
    }
}