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

        return scene.ResourceFactory.CreateBezierCurveGeometry(bezierCurve.Point0, bezierCurve.Point1, bezierCurve.Point2, bezierCurve.Point3);
    });

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((BezierCurve)obj).StrokeColor));

    private static readonly Resource StrokeStyleResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateStrokeStyle(new StrokeStyleProperties { StartCap = CapStyle.Round, EndCap = CapStyle.Round, LineJoin = LineJoin.Round }));

    private Vector2 _point0 = Vector2.Zero;
    private Vector2 _point1 = Vector2.Zero;
    private Vector2 _point2 = Vector2.Zero;
    private Vector2 _point3 = Vector2.Zero;
    private Color4 _strokeColor = Color4.Black;
    private float _strokeThickness = 1f;

    private Vector2 _startDragPosition;
    private Vector2 _startDragPoint0;
    private Vector2 _startDragPoint1;
    private Vector2 _startDragPoint2;
    private Vector2 _startDragPoint3;

    private static readonly AbstractNode[] AbstractNodes =
    {
        new Node<BezierCurve>(o => o.Point0, (o, p)=> o.Point0 = p),
        new Node<BezierCurve>(o => o.Point1, (o, p)=> o.Point1 = p),
        new Node<BezierCurve>(o => o.Point2, (o, p)=> o.Point2 = p),
        new Node<BezierCurve>(o => o.Point3, (o, p)=> o.Point3 = p)
    };

    public override AbstractNode[] Nodes => AbstractNodes;

    [Editable]
    public Vector2 Point0
    {
        get => _point0;
        set => SetAndUpdateResource(ref _point0, value, BezierCurveGeometryResource);
    }

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

    public override void StartDrag(Vector2 pos)
    {
        IsDragging = true;

        _startDragPosition = pos;
        _startDragPoint0 = Point0;
        _startDragPoint1 = Point1;
        _startDragPoint2 = Point2;
        _startDragPoint3 = Point3;
    }

    public override void Drag(Vector2 pos)
    {
        Point0 = _startDragPoint0 - _startDragPosition + pos;
        Point1 = _startDragPoint1 - _startDragPosition + pos;
        Point2 = _startDragPoint2 - _startDragPosition + pos;
        Point3 = _startDragPoint3 - _startDragPosition + pos;
    }

    public override void EndDrag()
    {
        IsDragging = false;
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<PathGeometry>(this, BezierCurveGeometryResource);

        return geometry.StrokeContainsPoint(pos, StrokeThickness, null, matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<PathGeometry>(this, BezierCurveGeometryResource);

        return geometry.Compare(rectGeometry, matrix, tolerance);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, BezierCurveGeometryResource, scene);
        var style = ResourceCache.GetOrUpdate<StrokeStyle>(this, StrokeStyleResource, scene);
        var brush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);

        renderTarget.DrawGeometry(geometry, brush, StrokeThickness / scene.Scale, style);
    }

    protected override void OnRenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush,
        StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, BezierCurveGeometryResource, scene);

        var strokeWidth = 1f / scene.Scale;

        renderTarget.DrawGeometry(geometry, selectionBrush, strokeWidth, selectionStyle);
        renderTarget.DrawLine(Point0, Point1, selectionBrush, strokeWidth, selectionStyle);
        renderTarget.DrawLine(Point1, Point2, selectionBrush, strokeWidth, selectionStyle);
        renderTarget.DrawLine(Point2, Point3, selectionBrush, strokeWidth, selectionStyle);
    }

    public override void Rotate(Vector2 offset)
    {
        var matrix = Matrix3x2.Transformation(1, 1, MathUtil.DegreesToRadians(90), offset.X, offset.Y);
        Point0 = Matrix3x2.TransformPoint(matrix, Point0 - offset);
        Point1 = Matrix3x2.TransformPoint(matrix, Point1 - offset);
        Point2 = Matrix3x2.TransformPoint(matrix, Point2 - offset);
        Point3 = Matrix3x2.TransformPoint(matrix, Point3 - offset);
    }
}