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
        using var brush = new SolidColorBrush(_renderTarget, rectangle.StrokeColor);

        _renderTarget.DrawRectangle(new RectangleF(rectangle.Location.X, rectangle.Location.Y, rectangle.Size.X, rectangle.Size.Y), brush, rectangle.StrokeWidth);
    }
}