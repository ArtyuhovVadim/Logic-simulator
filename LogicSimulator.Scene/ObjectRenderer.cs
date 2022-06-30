using SharpDX;
using SharpDX.Direct2D1;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.Scene;

public class ObjectRenderer
{
    private readonly RenderTarget _renderTarget;

    public ObjectRenderer(RenderTarget renderTarget) => _renderTarget = renderTarget;

    public void Render(Rectangle rectangle)
    {
        var strokeBrush = rectangle.GetResourceValue<SolidColorBrush>(Rectangle.StrokeBrushResource, _renderTarget);
        var geometry = rectangle.GetResourceValue<RectangleGeometry>(Rectangle.RectangleGeometryResource, _renderTarget);

        if (rectangle.IsFilled)
        {
            var fillBrush = rectangle.GetResourceValue<SolidColorBrush>(Rectangle.FillBrushResource, _renderTarget);
            _renderTarget.FillGeometry(geometry, fillBrush);
        }

        _renderTarget.DrawGeometry(geometry, strokeBrush, rectangle.StrokeThickness / _renderTarget.Transform.M11);

        if (rectangle.IsSelected)
        {
            var pos = new Vector2(rectangle.Width / 2, rectangle.Height / 2) + rectangle.Location;

            _renderTarget.DrawEllipse(new Ellipse(pos, 20, 20), strokeBrush, 1f / _renderTarget.Transform.M11);
        }
    }
}