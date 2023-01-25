using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class Arc : EditableSceneObject
{
    private static readonly Resource ArcGeometryResource = ResourceCache.Register((scene, obj) =>
    {
        var arc = (Arc)obj;
        arc._startAnglePos = MathHelper.GetPositionFromAngle(Vector2.Zero, arc.RadiusX, arc.RadiusY, -arc.StartAngle);
        arc._endAnglePos = MathHelper.GetPositionFromAngle(Vector2.Zero, arc.RadiusX, arc.RadiusY, -arc.EndAngle);
        return scene.ResourceFactory.CreateArcGeometry(Vector2.Zero, arc.RadiusX, arc.RadiusY, -arc.StartAngle, -arc.EndAngle);
    });

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((Arc)obj).StrokeColor));

    private Vector2 _startAnglePos;
    private Vector2 _endAnglePos;

    private float _radiusX;
    private float _radiusY;
    private float _startAngle;
    private float _endAngle = 180f;
    private Color4 _strokeColor = Color4.Black;
    private float _strokeThickness = 1f;

    private static readonly AbstractNode[] AbstractNodes =
    {
        new Node<Arc>(o =>
        {
            var start = -o.StartAngle;
            var end = -o.EndAngle;

            var ang = (end - start) / 2 + start;
            if (end < start) ang += 180;

            return o.LocalToWorldSpace(MathHelper.GetPositionFromAngle(Vector2.Zero, o.RadiusX, o.RadiusY, ang));
        }, (o, p) => o.RadiusX = o.RadiusY = o.WorldToLocalSpace(p).Length(), false),
        new Node<Arc>(o => o.LocalToWorldSpace(o._startAnglePos), (o, p) => 
            o.StartAngle = MathHelper.GetAngleForArc(Vector2.Zero, o.RadiusX, o.WorldToLocalSpace(p)), false),
        new Node<Arc>(o => o.LocalToWorldSpace(o._endAnglePos), (o, p) => 
            o.EndAngle = MathHelper.GetAngleForArc(Vector2.Zero, o.RadiusY, o.WorldToLocalSpace(p)), false),
    };

    public override AbstractNode[] Nodes => AbstractNodes;

    [Editable]
    public float RadiusX
    {
        get => _radiusX;
        set => SetAndUpdateResource(ref _radiusX, value, ArcGeometryResource);
    }

    [Editable]
    public float RadiusY
    {
        get => _radiusY;
        set => SetAndUpdateResource(ref _radiusY, value, ArcGeometryResource);
    }

    [Editable]
    public float StartAngle
    {
        get => _startAngle;
        set => SetAndUpdateResource(ref _startAngle, value, ArcGeometryResource);
    }

    [Editable]
    public float EndAngle
    {
        get => _endAngle;
        set => SetAndUpdateResource(ref _endAngle, value, ArcGeometryResource);
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
        InitializeResource(ArcGeometryResource);
        InitializeResource(StrokeBrushResource);
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<Geometry>(this, ArcGeometryResource);

        return geometry.StrokeContainsPoint(pos, StrokeThickness, null, TransformMatrix * matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<Geometry>(this, ArcGeometryResource);

        return geometry.Compare(rectGeometry, Matrix3x2.Invert(TransformMatrix) * matrix, tolerance);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var geometry = ResourceCache.GetOrUpdate<Geometry>(this, ArcGeometryResource, scene);
        var brush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);

        renderTarget.DrawGeometry(geometry, brush, StrokeThickness / scene.Scale);
    }

    protected override void OnRenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var strokeWidth = 1f / scene.Scale;

        renderTarget.DrawEllipse(new SharpDX.Direct2D1.Ellipse(Vector2.Zero, RadiusX, RadiusY), selectionBrush, strokeWidth, selectionStyle);
        renderTarget.DrawLine(Vector2.Zero, _startAnglePos, selectionBrush, strokeWidth, selectionStyle);
        renderTarget.DrawLine(Vector2.Zero, _endAnglePos, selectionBrush, strokeWidth, selectionStyle);
    }
}