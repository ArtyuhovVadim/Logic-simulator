using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class BezierCurve : EditableSceneObject
{
    private static readonly Resource BezierCurveGeometryResource = ResourceCache.Register((scene, obj) =>
    {
        var bezierCurve = (BezierCurve)obj;

        return scene.ResourceFactory.CreateBezierCurveGeometry(Vector2.Zero, bezierCurve.Point1, bezierCurve.Point2, bezierCurve.Point3);
    });

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((BezierCurve)obj).StrokeColor));

    private static readonly Resource StrokeStyleResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateStrokeStyle(new StrokeStyleProperties { StartCap = CapStyle.Round, EndCap = CapStyle.Round, LineJoin = LineJoin.Round }));

    private Vector2 _point1 = Vector2.Zero;
    private Vector2 _point2 = Vector2.Zero;
    private Vector2 _point3 = Vector2.Zero;
    private Color4 _strokeColor = Color4.Black;
    private float _strokeThickness = 1f;

    private static readonly AbstractNode[] AbstractNodes =
    {
        new Node<BezierCurve>(o => o.LocalToWorldSpace(Vector2.Zero), (o, p) =>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = p;
            o.Point1 -= localPos;
            o.Point2 -= localPos;
            o.Point3 -= localPos;
        }),
        new Node<BezierCurve>(o => o.LocalToWorldSpace(o.Point1), (o, p)=> o.Point1 = o.WorldToLocalSpace(p)),
        new Node<BezierCurve>(o => o.LocalToWorldSpace(o.Point2), (o, p)=> o.Point2 = o.WorldToLocalSpace(p)),
        new Node<BezierCurve>(o => o.LocalToWorldSpace(o.Point3), (o, p)=> o.Point3 = o.WorldToLocalSpace(p))
    };

    public override AbstractNode[] Nodes => AbstractNodes;

    [Editable]
    public Vector2 Point1
    {
        get => _point1;
        set => SetAndUpdateResource(ref _point1, value, BezierCurveGeometryResource);
    }

    [Editable]
    public Vector2 Point2
    {
        get => _point2;
        set => SetAndUpdateResource(ref _point2, value, BezierCurveGeometryResource);
    }

    [Editable]
    public Vector2 Point3
    {
        get => _point3;
        set => SetAndUpdateResource(ref _point3, value, BezierCurveGeometryResource);
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

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(BezierCurveGeometryResource);
        InitializeResource(StrokeBrushResource);
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<PathGeometry>(this, BezierCurveGeometryResource);

        return geometry.StrokeContainsPoint(pos, StrokeThickness, null, TransformMatrix * matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<PathGeometry>(this, BezierCurveGeometryResource);

        return geometry.Compare(rectGeometry, Matrix3x2.Invert(TransformMatrix) * matrix, tolerance);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, BezierCurveGeometryResource, scene);
        var style = ResourceCache.GetOrUpdate<StrokeStyle>(this, StrokeStyleResource, scene);
        var brush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);

        renderTarget.DrawGeometry(geometry, brush, StrokeThickness / scene.Scale, style);
    }

    protected override void OnRenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, BezierCurveGeometryResource, scene);

        var strokeWidth = 1f / scene.Scale;

        renderTarget.DrawGeometry(geometry, selectionBrush, strokeWidth, selectionStyle);
        renderTarget.DrawLine(Vector2.Zero, Point1, selectionBrush, strokeWidth, selectionStyle);
        renderTarget.DrawLine(Point1, Point2, selectionBrush, strokeWidth, selectionStyle);
        renderTarget.DrawLine(Point2, Point3, selectionBrush, strokeWidth, selectionStyle);
    }
}