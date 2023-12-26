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

public class EllipseView : EditableSceneObjectView, IStroked
{
    public static readonly IResource FillBrushResource =
        ResourceCache.Register<EllipseView>((factory, user) => factory.CreateSolidColorBrush(user.FillColor.ToColor4()));

    public static readonly IResource StrokeBrushResource =
        ResourceCache.Register<EllipseView>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

    public static readonly IResource GeometryResource =
        ResourceCache.Register<EllipseView>((factory, user) => factory.CreateEllipseGeometry(new Ellipse { RadiusX = user.RadiusX, RadiusY = user.RadiusY }));

    private static readonly AbstractNode[] AbstractNodes =
    [
        new Node<EllipseView>(o => o.LocalToWorldSpace(new Vector2(o.RadiusX, 0)), (o, p) => o.RadiusX = Math.Abs(o.WorldToLocalSpace(p).X)),
        new Node<EllipseView>(o => o.LocalToWorldSpace(new Vector2(0, -o.RadiusY)), (o, p) => o.RadiusY = Math.Abs(o.WorldToLocalSpace(p).Y))
    ];

    public override IEnumerable<AbstractNode> Nodes => AbstractNodes;

    #region RadiusX

    public float RadiusX
    {
        get => (float)GetValue(RadiusXProperty);
        set => SetValue(RadiusXProperty, value);
    }

    public static readonly DependencyProperty RadiusXProperty =
        DependencyProperty.Register(nameof(RadiusX), typeof(float), typeof(EllipseView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region RadiusY

    public float RadiusY
    {
        get => (float)GetValue(RadiusYProperty);
        set => SetValue(RadiusYProperty, value);
    }

    public static readonly DependencyProperty RadiusYProperty =
        DependencyProperty.Register(nameof(RadiusY), typeof(float), typeof(EllipseView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region FillColor

    public Color FillColor
    {
        get => (Color)GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }

    public static readonly DependencyProperty FillColorProperty =
        DependencyProperty.Register(nameof(FillColor), typeof(Color), typeof(EllipseView), new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFillColorChanged));

    private static void OnFillColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not EllipseView ellipseView) return;

        ellipseView.ThrowIfDisposed();

        ellipseView.Cache?.Update(ellipseView, FillBrushResource);

        ellipseView.MakeDirty();
    }

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(EllipseView), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

    private static void OnStrokeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not EllipseView ellipseView) return;

        ellipseView.ThrowIfDisposed();

        ellipseView.Cache?.Update(ellipseView, StrokeBrushResource);

        ellipseView.MakeDirty();
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => (float)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(EllipseView), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => (StrokeThicknessType)GetValue(StrokeThicknessTypeProperty);
        set => SetValue(StrokeThicknessTypeProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessTypeProperty =
        DependencyProperty.Register(nameof(StrokeThicknessType), typeof(StrokeThicknessType), typeof(EllipseView), new FrameworkPropertyMetadata(StrokeThicknessType.Smallest, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region IsFilled

    public bool IsFilled
    {
        get => (bool)GetValue(IsFilledProperty);
        set => SetValue(IsFilledProperty, value);
    }

    public static readonly DependencyProperty IsFilledProperty =
        DependencyProperty.Register(nameof(IsFilled), typeof(bool), typeof(EllipseView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    public override bool HitTest(Vector2 pos, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<EllipseGeometry>(this, GeometryResource);

        return IsFilled ?
            geometry.FillContainsPoint(pos, TransformMatrix * worldTransform, tolerance) :
            geometry.StrokeContainsPoint(pos, this.GetStrokeThickness(), null, TransformMatrix * worldTransform, tolerance);
    }

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<EllipseGeometry>(this, GeometryResource);
        return geometry.Compare(inputGeometry, Matrix3x2.Invert(TransformMatrix) * worldTransform, tolerance);
    }

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var ellipse = new Ellipse
        {
            RadiusX = RadiusX,
            RadiusY = RadiusY
        };

        if (IsFilled)
        {
            var fillBrush = Cache.Get<SolidColorBrush>(this, FillBrushResource);
            context.DrawingContext.FillEllipse(ellipse, fillBrush);
        }

        var strokeBrush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);

        context.DrawingContext.DrawEllipse(ellipse, strokeBrush, this.GetStrokeThickness(scene));
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get<SolidColorBrush>(SelectionBrushStaticResource);
        var style = Cache.Get<StrokeStyle>(SelectionStyleStaticResource);

        var ellipse = new Ellipse
        {
            RadiusX = RadiusX,
            RadiusY = RadiusY
        };

        context.DrawingContext.DrawEllipse(ellipse, brush, 1f / scene.Scale, style);
    }

    private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not EllipseView ellipseView) return;

        ellipseView.ThrowIfDisposed();

        ellipseView.Cache?.Update(ellipseView, GeometryResource);

        ellipseView.MakeDirty();
    }
}