using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene;

public class ResourceFactory
{
    private readonly SceneRenderer _sceneRenderer;

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
}