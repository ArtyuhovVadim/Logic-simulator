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

public class ArcView : EditableSceneObjectView, IStroked
{
    private Vector2 _startAnglePos;
    private Vector2 _endAnglePos;

    public static readonly IStaticResource<StrokeStyle> StrokeStyleResource =
        ResourceCache.RegisterStatic(factory => factory.CreateStrokeStyle(new StrokeStyleProperties { StartCap = CapStyle.Round, EndCap = CapStyle.Round, LineJoin = LineJoin.Round }));

    public static readonly IResource<ArcView, SolidColorBrush> StrokeBrushResource =
        ResourceCache.Register<ArcView, SolidColorBrush>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

    public static readonly IResource<ArcView, Geometry> GeometryResource = ResourceCache.Register<ArcView, Geometry>((factory, user) =>
    {
        user._startAnglePos = MathHelper.GetPositionFromAngle(Vector2.Zero, user.RadiusX, user.RadiusY, -user.StartAngle);
        user._endAnglePos = MathHelper.GetPositionFromAngle(Vector2.Zero, user.RadiusX, user.RadiusY, -user.EndAngle);
        return factory.CreateArcGeometry(Vector2.Zero, user.RadiusX, user.RadiusY, -user.StartAngle, -user.EndAngle);
    });

    private static readonly AbstractNode[] AbstractNodes =
    [
        new Node<ArcView>(o =>
        {
            var start = -o.StartAngle;
            var end = -o.EndAngle;

            var ang = (end - start) / 2 + start;
            if (end < start) ang += 180;

            return o.LocalToWorldSpace(MathHelper.GetPositionFromAngle(Vector2.Zero, o.RadiusX, o.RadiusY, ang));
        }, (o, p) => o.RadiusX = o.RadiusY = o.WorldToLocalSpace(p).Length(), false),
        new Node<ArcView>(o => o.LocalToWorldSpace(o._startAnglePos), (o, p) =>
            o.StartAngle = MathHelper.GetAngleForArc(Vector2.Zero, o.RadiusX, o.WorldToLocalSpace(p)), false),
        new Node<ArcView>(o => o.LocalToWorldSpace(o._endAnglePos), (o, p) =>
            o.EndAngle = MathHelper.GetAngleForArc(Vector2.Zero, o.RadiusY, o.WorldToLocalSpace(p)), false)
    ];

    public override IEnumerable<AbstractNode> Nodes => AbstractNodes;

    #region StartAngle

    public float StartAngle
    {
        get => (float)GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }

    public static readonly DependencyProperty StartAngleProperty =
        DependencyProperty.Register(nameof(StartAngle), typeof(float), typeof(ArcView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region EndAngle

    public float EndAngle
    {
        get => (float)GetValue(EndAngleProperty);
        set => SetValue(EndAngleProperty, value);
    }

    public static readonly DependencyProperty EndAngleProperty =
        DependencyProperty.Register(nameof(EndAngle), typeof(float), typeof(ArcView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region RadiusX

    public float RadiusX
    {
        get => (float)GetValue(RadiusXProperty);
        set => SetValue(RadiusXProperty, value);
    }

    public static readonly DependencyProperty RadiusXProperty =
        DependencyProperty.Register(nameof(RadiusX), typeof(float), typeof(ArcView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region RadiusY

    public float RadiusY
    {
        get => (float)GetValue(RadiusYProperty);
        set => SetValue(RadiusYProperty, value);
    }

    public static readonly DependencyProperty RadiusYProperty =
        DependencyProperty.Register(nameof(RadiusY), typeof(float), typeof(ArcView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(ArcView), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

    private static void OnStrokeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ArcView arcView) return;

        arcView.ThrowIfDisposed();

        arcView.Cache?.Update(arcView, StrokeBrushResource);

        arcView.MakeDirty();
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => (float)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(ArcView), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => (StrokeThicknessType)GetValue(StrokeThicknessTypeProperty);
        set => SetValue(StrokeThicknessTypeProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessTypeProperty =
        DependencyProperty.Register(nameof(StrokeThicknessType), typeof(StrokeThicknessType), typeof(ArcView), new FrameworkPropertyMetadata(StrokeThicknessType.Smallest, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    protected override RectangleF OnWorldBoundsChanged() =>
        Cache?.Get(this, GeometryResource).GetWidenedBounds(this.GetStrokeThickness(), null, WorldTransformMatrix, D2D1.DefaultFlatteningTolerance).ToRect() ?? RectangleF.Empty;

    public override bool HitTest(Vector2 pos, Matrix3x2 transform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get(this, GeometryResource);
        return geometry.StrokeContainsPoint(pos, this.GetStrokeThickness(), null, WorldTransformMatrix * transform, tolerance);
    }

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 transform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get(this, GeometryResource);
        return geometry.Compare(inputGeometry, Matrix3x2.Invert(WorldTransformMatrix * transform), tolerance);
    }

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var geometry = Cache.Get(this, GeometryResource);
        var strokeBrush = Cache.Get(this, StrokeBrushResource);
        var style = Cache.Get(StrokeStyleResource);

        context.DrawingContext.DrawGeometry(geometry, strokeBrush, this.GetStrokeThickness(scene), style);
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get(SelectionBrushStaticResource);
        var style = Cache.Get(SelectionStyleStaticResource);

        var strokeWidth = 1f / scene.Scale;

        context.DrawingContext.DrawEllipse(new Ellipse(Vector2.Zero, RadiusX, RadiusY), brush, strokeWidth, style);
        context.DrawingContext.DrawLine(Vector2.Zero, _startAnglePos, brush, strokeWidth, style);
        context.DrawingContext.DrawLine(Vector2.Zero, _endAnglePos, brush, strokeWidth, style);
    }

    protected override void OnCacheChanged(ResourceCache cache)
    {
        base.OnCacheChanged(cache);

        Cache.Update(this, GeometryResource);
        Cache.Update(this, StrokeBrushResource);
        WorldBoundsChanged();
    }

    private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ArcView arcView) return;

        arcView.ThrowIfDisposed();

        arcView.Cache?.Update(arcView, GeometryResource);
        arcView.WorldBoundsChanged();

        arcView.MakeDirty();
    }
}