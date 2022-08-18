using System;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class Ellipse : EditableSceneObject
{
    public static readonly Resource EllipseGeometryResource = Resource.Register<Ellipse, EllipseGeometry>(nameof(EllipseGeometryResource), (target, o) =>
    {
        var ellipse = (Ellipse)o;

        return new EllipseGeometry(target.Factory, new SharpDX.Direct2D1.Ellipse(ellipse.Center, ellipse.RadiusX, ellipse.RadiusY));
    });

    public static readonly Resource FillBrushResource = Resource.Register<Ellipse, SolidColorBrush>(nameof(FillBrushResource), (target, o) =>
        new SolidColorBrush(target, ((Ellipse)o).FillColor));

    public static readonly Resource StrokeBrushResource = Resource.Register<Ellipse, SolidColorBrush>(nameof(StrokeBrushResource), (target, o) =>
        new SolidColorBrush(target, ((Ellipse)o).StrokeColor));

    private Vector2 _center = Vector2.Zero;
    private float _radiusX;
    private float _radiusY;
    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;

    private Vector2 _startDragPosition;
    private Vector2 _startDragCenter;

    public Ellipse()
    {
        Nodes = new Node[]
        {
            new(() => Center + new Vector2(RadiusX, 0), pos => RadiusX = Math.Abs((pos - Center).X)),
            new(() => Center + new Vector2(0, -RadiusY), pos => RadiusY = Math.Abs((pos - Center).Y))
        };
    }

    public override Node[] Nodes { get; }

    public Vector2 Center
    {
        get => _center;
        set
        {
            _center = value;
            RequireUpdate(EllipseGeometryResource);
        }
    }

    public float RadiusX
    {
        get => _radiusX;
        set
        {
            _radiusX = value;
            RequireUpdate(EllipseGeometryResource);
        }
    }

    public float RadiusY
    {
        get => _radiusY;
        set
        {
            _radiusY = value;
            RequireUpdate(EllipseGeometryResource);
        }
    }

    public Color4 FillColor
    {
        get => _fillColor;
        set
        {
            _fillColor = value;
            RequireUpdate(FillBrushResource);
        }
    }

    public Color4 StrokeColor
    {
        get => _strokeColor;
        set
        {
            _strokeColor = value;
            RequireUpdate(StrokeBrushResource);
        }
    }

    public float StrokeThickness { get; set; } = 1f;

    public bool IsFilled { get; set; }

    public override void StartDrag(Vector2 pos)
    {
        IsDragging = true;

        _startDragPosition = pos;
        _startDragCenter = Center;
    }

    public override void Drag(Vector2 pos)
    {
        Center = _startDragCenter - _startDragPosition + pos;
    }

    public override void EndDrag()
    {
        IsDragging = false;
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = GetCashedResourceValue<EllipseGeometry>(EllipseGeometryResource);

        return IsFilled ? geometry.FillContainsPoint(pos, matrix, tolerance) :
                            geometry.StrokeContainsPoint(pos, StrokeThickness, null, matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = GetCashedResourceValue<EllipseGeometry>(EllipseGeometryResource);

        return geometry.Compare(rectGeometry, matrix, tolerance);
    }

    public override void Render(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = GetResourceValue<SolidColorBrush>(Ellipse.StrokeBrushResource, renderTarget);
        var geometry = GetResourceValue<EllipseGeometry>(Ellipse.EllipseGeometryResource, renderTarget);

        if (IsFilled)
        {
            var fillBrush = GetResourceValue<SolidColorBrush>(Ellipse.FillBrushResource, renderTarget);
            renderTarget.FillGeometry(geometry, fillBrush);
        }

        renderTarget.DrawGeometry(geometry, strokeBrush, StrokeThickness / scene.Scale);
    }

    public override void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var geometry = GetResourceValue<EllipseGeometry>(EllipseGeometryResource, renderTarget);
        
        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
        renderTarget.DrawLine(Center, Center + new Vector2(RadiusX, 0), selectionBrush, 1f / scene.Scale, selectionStyle);
        renderTarget.DrawLine(Center, Center + new Vector2(0, -RadiusY), selectionBrush, 1f / scene.Scale, selectionStyle);
    }
}