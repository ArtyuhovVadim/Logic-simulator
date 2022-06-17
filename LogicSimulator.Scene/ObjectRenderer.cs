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
        using var fillBrush = new SolidColorBrush(_renderTarget, rectangle.FillColor);
        using var strokeBrush = new SolidColorBrush(_renderTarget, rectangle.StrokeColor);

        var rect = new RectangleF(rectangle.Location.X, rectangle.Location.Y, rectangle.Width, rectangle.Height);

        using var geometry = new RectangleGeometry(_renderTarget.Factory, rect);

        _renderTarget.FillGeometry(geometry, fillBrush);
        _renderTarget.DrawGeometry(geometry, strokeBrush, rectangle.StrokeThickness);
    }
}