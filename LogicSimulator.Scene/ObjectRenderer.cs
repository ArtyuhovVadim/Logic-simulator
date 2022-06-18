﻿using SharpDX.Direct2D1;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.Scene;

public class ObjectRenderer
{
    private readonly RenderTarget _renderTarget;

    public ObjectRenderer(RenderTarget renderTarget) => _renderTarget = renderTarget;

    public void Render(Rectangle rectangle)
    {
        var fillBrush = rectangle.GetResourceValue<SolidColorBrush>(Rectangle.FillBrushResource, _renderTarget);
        var strokeBrush = rectangle.GetResourceValue<SolidColorBrush>(Rectangle.StrokeBrushResource, _renderTarget);
        var geometry = rectangle.GetResourceValue<RectangleGeometry>(Rectangle.RectangleGeometryResource, _renderTarget);

        _renderTarget.FillGeometry(geometry, fillBrush);
        _renderTarget.DrawGeometry(geometry, strokeBrush, rectangle.StrokeThickness);
    }
}