using System;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class Ellipse : EditableSceneObject
{
    private static readonly Resource EllipseGeometryResource = ResourceCache.Register((scene, obj) =>
    {
        var ellipse = (Ellipse)obj;

        return scene.ResourceFactory.CreateEllipseGeometry(new SharpDX.Direct2D1.Ellipse(ellipse.Center, ellipse.RadiusX, ellipse.RadiusY));
    });

    private static readonly Resource FillBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((Ellipse)obj).FillColor));

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((Ellipse)obj).StrokeColor));

    private Vector2 _center = Vector2.Zero;
    private float _radiusX;
    private float _radiusY;
    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;
    private float _strokeThickness = 1f;
    private bool _isFilled = true;

    private Vector2 _startDragPosition;
    private Vector2 _startDragCenter;

    private static readonly AbstractNode[] AbstractNodes =
    {
        new Node<Ellipse>(o => o.Center + new Vector2(o.RadiusX, 0), (o, p)=> o.RadiusX = Math.Abs((p - o.Center).X)),
        new Node<Ellipse>(o => o.Center + new Vector2(0, -o.RadiusY), (o, p)=> o.RadiusY = Math.Abs((p - o.Center).Y))
    };

    public override AbstractNode[] Nodes => AbstractNodes;

    [Editable]
    public Vector2 Center
    {
        get => _center;
        set => SetAndUpdateResource(ref _center, value, EllipseGeometryResource);
    }

    [Editable]
    public float RadiusX
    {
        get => _radiusX;
        set => SetAndUpdateResource(ref _radiusX, value, EllipseGeometryResource);
    }

    [Editable]
    public float RadiusY
    {
        get => _radiusY;
        set => SetAndUpdateResource(ref _radiusY, value, EllipseGeometryResource);
    }

    [Editable]
    public Color4 FillColor
    {
        get => _fillColor;
        set => SetAndUpdateResource(ref _fillColor, value, FillBrushResource);
    }

    [Editable]
    public Color4 StrokeColor
    {
        get => _strokeColor;
        set => SetAndUpdateResource(ref _strokeColor, value, StrokeBrushResource);
    }

    [Editable]
    public float StrokeThickness
    {
        get => _strokeThickness;
        set => SetAndRequestRender(ref _strokeThickness, value);
    }

    [Editable]
    public bool IsFilled
    {
        get => _isFilled;
        set => SetAndRequestRender(ref _isFilled, value);
    }

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(EllipseGeometryResource);
        InitializeResource(FillBrushResource);
        InitializeResource(StrokeBrushResource);
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

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
        var geometry = ResourceCache.GetOrUpdate<EllipseGeometry>(this, EllipseGeometryResource, scene);

        if (IsFilled)
        {
            var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, scene);
            renderTarget.FillGeometry(geometry, fillBrush);
        }

        renderTarget.DrawGeometry(geometry, strokeBrush, StrokeThickness / scene.Scale);
    }

    public override void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<EllipseGeometry>(this, EllipseGeometryResource, scene);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
        renderTarget.DrawLine(Center, Center + new Vector2(RadiusX, 0), selectionBrush, 1f / scene.Scale, selectionStyle);
        renderTarget.DrawLine(Center, Center + new Vector2(0, -RadiusY), selectionBrush, 1f / scene.Scale, selectionStyle);
    }
}