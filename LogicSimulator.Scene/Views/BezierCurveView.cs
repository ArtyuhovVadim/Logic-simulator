using System.Windows;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using Color = System.Windows.Media.Color;
using Colors = System.Windows.Media.Colors;

namespace LogicSimulator.Scene.Views;

public class BezierCurveView : EditableSceneObjectView, IStroked
{
    public static readonly IResource GeometryResource =
        ResourceCache.Register<BezierCurveView>((factory, user) => factory.CreateBezierCurveGeometry(Vector2.Zero, user.Point1, user.Point2, user.Point3));

    public static readonly IResource StrokeBrushResource =
        ResourceCache.Register<BezierCurveView>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

    public static readonly IResource StrokeStyleResource =
        ResourceCache.Register<BezierCurveView>((factory, _) => factory.CreateStrokeStyle(new StrokeStyleProperties { StartCap = CapStyle.Round, EndCap = CapStyle.Round, LineJoin = LineJoin.Round }));

    private static readonly AbstractNode[] AbstractNodes =
    [
        new Node<BezierCurveView>(o => o.LocalToWorldSpace(Vector2.Zero), (o, p) =>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = p;
            o.Point1 -= localPos;
            o.Point2 -= localPos;
            o.Point3 -= localPos;
        }),
        new Node<BezierCurveView>(o => o.LocalToWorldSpace(o.Point1), (o, p)=> o.Point1 = o.WorldToLocalSpace(p)),
        new Node<BezierCurveView>(o => o.LocalToWorldSpace(o.Point2), (o, p)=> o.Point2 = o.WorldToLocalSpace(p)),
        new Node<BezierCurveView>(o => o.LocalToWorldSpace(o.Point3), (o, p)=> o.Point3 = o.WorldToLocalSpace(p))
    ];

    public override IEnumerable<AbstractNode> Nodes => AbstractNodes;

    #region Point1

    public Vector2 Point1
    {
        get => (Vector2)GetValue(Point1Property);
        set => SetValue(Point1Property, value);
    }

    public static readonly DependencyProperty Point1Property =
        DependencyProperty.Register(nameof(Point1), typeof(Vector2), typeof(BezierCurveView), new FrameworkPropertyMetadata(default(Vector2), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region Point2

    public Vector2 Point2
    {
        get => (Vector2)GetValue(Point2Property);
        set => SetValue(Point2Property, value);
    }

    public static readonly DependencyProperty Point2Property =
        DependencyProperty.Register(nameof(Point2), typeof(Vector2), typeof(BezierCurveView), new FrameworkPropertyMetadata(default(Vector2), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region Point3

    public Vector2 Point3
    {
        get => (Vector2)GetValue(Point3Property);
        set => SetValue(Point3Property, value);
    }

    public static readonly DependencyProperty Point3Property =
        DependencyProperty.Register(nameof(Point3), typeof(Vector2), typeof(BezierCurveView), new FrameworkPropertyMetadata(default(Vector2), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(BezierCurveView), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

    private static void OnStrokeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not BezierCurveView bezierCurveView) return;

        bezierCurveView.ThrowIfDisposed();

        bezierCurveView.Cache?.Update(bezierCurveView, StrokeBrushResource);

        bezierCurveView.MakeDirty();
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => (float)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(BezierCurveView), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => (StrokeThicknessType)GetValue(StrokeThicknessTypeProperty);
        set => SetValue(StrokeThicknessTypeProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessTypeProperty =
        DependencyProperty.Register(nameof(StrokeThicknessType), typeof(StrokeThicknessType), typeof(BezierCurveView), new FrameworkPropertyMetadata(StrokeThicknessType.Smallest, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    public override bool HitTest(Vector2 pos, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<PathGeometry>(this, GeometryResource);

        return geometry.StrokeContainsPoint(pos, this.GetStrokeThickness(), null, TransformMatrix * worldTransform, tolerance);
    }

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<PathGeometry>(this, GeometryResource);

        return geometry.Compare(inputGeometry, Matrix3x2.Invert(TransformMatrix) * worldTransform, tolerance);
    }

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var geometry = Cache.Get<PathGeometry>(this, GeometryResource);
        var strokeBrush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);
        var style = Cache.Get<StrokeStyle>(this, StrokeStyleResource);

        context.DrawingContext.DrawGeometry(geometry, strokeBrush, this.GetStrokeThickness(scene), style);
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get<SolidColorBrush>(SelectionBrushStaticResource);
        var style = Cache.Get<StrokeStyle>(SelectionStyleStaticResource);
        var geometry = Cache.Get<PathGeometry>(this, GeometryResource);

        var strokeWidth = 1f / scene.Scale;

        context.DrawingContext.DrawGeometry(geometry, brush, strokeWidth, style);
        context.DrawingContext.DrawLine(Vector2.Zero, Point1, brush, strokeWidth, style);
        context.DrawingContext.DrawLine(Point1, Point2, brush, strokeWidth, style);
        context.DrawingContext.DrawLine(Point2, Point3, brush, strokeWidth, style);
    }

    private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not BezierCurveView bezierCurveView) return;

        bezierCurveView.ThrowIfDisposed();

        bezierCurveView.Cache?.Update(bezierCurveView, GeometryResource);

        bezierCurveView.MakeDirty();
    }
}