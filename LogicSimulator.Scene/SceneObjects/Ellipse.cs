using System;
using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class Ellipse : EditableSceneObject
{
    private static readonly Resource EllipseGeometryResource = ResourceCache.Register((scene, obj) =>
    {
        var ellipse = (Ellipse)obj;

        return scene.ResourceFactory.CreateEllipseGeometry(new SharpDX.Direct2D1.Ellipse(Vector2.Zero, ellipse.RadiusX, ellipse.RadiusY));
    });

    private static readonly Resource FillBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((Ellipse)obj).FillColor));

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((Ellipse)obj).StrokeColor));

    private float _radiusX;
    private float _radiusY;
    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;
    private float _strokeThickness = 1f;
    private bool _isFilled = true;

    private static readonly AbstractNode[] AbstractNodes =
    {
        new Node<Ellipse>(o => o.LocalToWorldSpace(new Vector2(o.RadiusX, 0)), (o, p) => o.RadiusX = Math.Abs(o.WorldToLocalSpace(p).X)),
        new Node<Ellipse>(o => o.LocalToWorldSpace(new Vector2(0, -o.RadiusY)), (o, p) => o.RadiusY = Math.Abs(o.WorldToLocalSpace(p).Y))
    };

    public override AbstractNode[] Nodes => AbstractNodes;

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

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<EllipseGeometry>(this, EllipseGeometryResource);

        return IsFilled ? geometry.FillContainsPoint(pos, TransformMatrix * matrix, tolerance) :
                            geometry.StrokeContainsPoint(pos, StrokeThickness, null, TransformMatrix * matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<EllipseGeometry>(this, EllipseGeometryResource);

        return geometry.Compare(rectGeometry, Matrix3x2.Invert(TransformMatrix) * matrix, tolerance);
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

    protected override void OnRenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush,
        StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<EllipseGeometry>(this, EllipseGeometryResource, scene);

        var strokeWidth = 1f / scene.Scale;

        renderTarget.DrawGeometry(geometry, selectionBrush, strokeWidth, selectionStyle);
        renderTarget.DrawLine(Location, Location + new Vector2(RadiusX, 0), selectionBrush, strokeWidth, selectionStyle);
        renderTarget.DrawLine(Location, Location + new Vector2(0, -RadiusY), selectionBrush, strokeWidth, selectionStyle);
    }
}