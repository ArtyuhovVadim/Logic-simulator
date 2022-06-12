using System;
using SharpDX;
using SharpDX.Direct2D1;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.Scene;

public class ObjectRenderer : IDisposable
{
    private readonly RenderTarget _renderTarget;

    public ObjectRenderer(RenderTarget renderTarget) => _renderTarget = renderTarget;

    public void Render(Rectangle rectangle)
    {
        var fillBrush = rectangle.GetResourceValue<SolidColorBrush>(Rectangle.FillBrushResource, this);
        var strokeBrush = rectangle.GetResourceValue<SolidColorBrush>(Rectangle.StrokeBrushResource, this);
        var geometry = rectangle.GetResourceValue<RectangleGeometry>(Rectangle.RectangleGeometryResource, this);

        //_renderTarget.FillGeometry(geometry, fillBrush);
        //_renderTarget.DrawGeometry(geometry, strokeBrush, rectangle.StrokeWidth);

        var rect = new RectangleF(rectangle.Location.X, rectangle.Location.Y, rectangle.Width, rectangle.Height);

        _renderTarget.FillRectangle(rect, fillBrush);
        _renderTarget.DrawRectangle(rect, strokeBrush, rectangle.StrokeWidth);
    }

    public SolidColorBrush CreateSolidColorBrush(Color4 color) => new(_renderTarget, color);

    public RectangleGeometry CreateRectangleGeometry(RectangleF rect) => new(_renderTarget.Factory, rect);

    public void Dispose()
    {
        _renderTarget?.Dispose();
    }
}