﻿using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace LogicSimulator.Scene;

public class ResourceFactory
{
    private readonly SceneRenderer _sceneRenderer;

    private PathGeometry _tempPathGeometry;
    private GeometrySink _tempGeometrySink;

    public ResourceFactory(SceneRenderer sceneRenderer)
    {
        _sceneRenderer = sceneRenderer;
    }

    public SolidColorBrush CreateSolidColorBrush(in Color4 color)
    {
        return new SolidColorBrush(_sceneRenderer.RenderTarget, color);
    }

    public LinearGradientBrush CreateLinearGradientBrush(in LinearGradientBrushProperties properties, in GradientStop[] gradientStops)
    {
        var gradientStopCollection = new GradientStopCollection(_sceneRenderer.RenderTarget, gradientStops);

        var linearGradientBrush = new LinearGradientBrush(_sceneRenderer.RenderTarget, properties, gradientStopCollection);

        gradientStopCollection.Dispose();

        return linearGradientBrush;
    }

    public RectangleGeometry CreateRectangleGeometry(in RectangleF rectangle)
    {
        return new RectangleGeometry(_sceneRenderer.Factory, rectangle);
    }

    public RoundedRectangleGeometry CreateRoundedRectangleGeometry(in RoundedRectangle roundedRectangle)
    {
        return new RoundedRectangleGeometry(_sceneRenderer.Factory, roundedRectangle);
    }

    public EllipseGeometry CreateEllipseGeometry(in Ellipse ellipse)
    {
        return new EllipseGeometry(_sceneRenderer.Factory, ellipse);
    }

    public PathGeometry CreatePolylineGeometry(List<Vector2> vertices)
    {
        var path = new PathGeometry(_sceneRenderer.Factory);

        var sink = path.Open();
        sink.BeginFigure(vertices.First(), FigureBegin.Hollow);

        for (var i = 1; i < vertices.Count; i++)
        {
            sink.AddLine(vertices[i]);
        }

        sink.EndFigure(FigureEnd.Open);
        sink.Close();
        sink.Dispose();

        return path;
    }

    public PathGeometry CreateBezierCurveGeometry(in Vector2 p0, in Vector2 p1, in Vector2 p2, in Vector2 p3)
    {
        var pathGeometry = new PathGeometry(_sceneRenderer.Factory);
        var sink = pathGeometry.Open();

        sink.BeginFigure(p0, FigureBegin.Hollow);
        sink.AddBezier(new BezierSegment { Point1 = p1, Point2 = p2, Point3 = p3 });
        sink.EndFigure(FigureEnd.Open);

        sink.Close();
        sink.Dispose();

        return pathGeometry;
    }

    public StrokeStyle CreateStrokeStyle(in StrokeStyleProperties properties, in float[] dashes)
    {
        return new StrokeStyle(_sceneRenderer.Factory, properties, dashes);
    }

    public StrokeStyle CreateStrokeStyle(in StrokeStyleProperties properties)
    {
        return new StrokeStyle(_sceneRenderer.Factory, properties);
    }

    public TextFormat CreateTextFormat(in string fontFamily, in FontWeight fontWeight, in FontStyle fontStyle, in FontStretch fontStretch, in float fontSize)
    {
        return new TextFormat(_sceneRenderer.TextFactory, fontFamily, fontWeight, fontStyle, fontStretch, fontSize);
    }

    public TextLayout CreateTextLayout(in string text, TextFormat textFormat)
    {
        return new TextLayout(_sceneRenderer.TextFactory, text, textFormat, float.MaxValue, float.MaxValue);
    }

    public GeometrySink BeginPathGeometry()
    {
        _tempPathGeometry = new PathGeometry(_sceneRenderer.Factory);
        _tempGeometrySink = _tempPathGeometry.Open();

        return _tempGeometrySink;
    }

    public PathGeometry EndPathGeometry()
    {
        _tempGeometrySink.Close();
        _tempGeometrySink.Dispose();

        var tmp = _tempPathGeometry;

        _tempGeometrySink = null;
        _tempPathGeometry = null;

        return tmp;
    }
}