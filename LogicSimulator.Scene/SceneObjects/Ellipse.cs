using System;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class Ellipse : EditableSceneObject
{
    public static readonly Resource EllipseGeometryResource = ResourceCache.Register((target, o) =>
    {
        var ellipse = (Ellipse)o;

        return new EllipseGeometry(target.Factory, new SharpDX.Direct2D1.Ellipse(ellipse.Center, ellipse.RadiusX, ellipse.RadiusY));
    });

    public static readonly Resource FillBrushResource = ResourceCache.Register((target, o) =>
        new SolidColorBrush(target, ((Ellipse)o).FillColor));

    public static readonly Resource StrokeBrushResource = ResourceCache.Register((target, o) =>
        new SolidColorBrush(target, ((Ellipse)o).StrokeColor));

    private Vector2 _center = Vector2.Zero;
    private float _radiusX;
    private float _radiusY;
    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;
    private float _strokeThickness = 1f;
    private bool _isFilled;

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
            ResourceCache.RequestUpdate(this, EllipseGeometryResource);
        }
    }

    public float RadiusX
    {
        get => _radiusX;
        set
        {
            _radiusX = value;
            ResourceCache.RequestUpdate(this, EllipseGeometryResource);
        }
    }

    public float RadiusY
    {
        get => _radiusY;
        set
        {
            _radiusY = value;
            ResourceCache.RequestUpdate(this, EllipseGeometryResource);
        }
    }

    public Color4 FillColor
    {
        get => _fillColor;
        set
        {
            _fillColor = value;
            ResourceCache.RequestUpdate(this, FillBrushResource);
        }
    }

    public Color4 StrokeColor
    {
        get => _strokeColor;
        set
        {
            _strokeColor = value;
            ResourceCache.RequestUpdate(this, StrokeBrushResource);
        }
    }

    public float StrokeThickness
    {
        get => _strokeThickness;
        set
        {
            _strokeThickness = value;
            RenderNotifier.RequestRender(this);
        }
    }

    public bool IsFilled
    {
        get => _isFilled;
        set
        {
            _isFilled = value;
            RenderNotifier.RequestRender(this);
        }
    }

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
        var geometry = ResourceCache.GetCached<EllipseGeometry>(this, EllipseGeometryResource);

        return IsFilled ? geometry.FillContainsPoint(pos, matrix, tolerance) :
                            geometry.StrokeContainsPoint(pos, StrokeThickness, null, matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<EllipseGeometry>(this, EllipseGeometryResource);

        return geometry.Compare(rectGeometry, matrix, tolerance);
    }

    public override void Render(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, renderTarget);
        var geometry = ResourceCache.GetOrUpdate<EllipseGeometry>(this, EllipseGeometryResource, renderTarget);

        if (IsFilled)
        {
            var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, renderTarget);
            renderTarget.FillGeometry(geometry, fillBrush);
        }

        renderTarget.DrawGeometry(geometry, strokeBrush, StrokeThickness / scene.Scale);
    }

    public override void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<EllipseGeometry>(this, EllipseGeometryResource, renderTarget);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
        renderTarget.DrawLine(Center, Center + new Vector2(RadiusX, 0), selectionBrush, 1f / scene.Scale, selectionStyle);
        renderTarget.DrawLine(Center, Center + new Vector2(0, -RadiusY), selectionBrush, 1f / scene.Scale, selectionStyle);
    }
}