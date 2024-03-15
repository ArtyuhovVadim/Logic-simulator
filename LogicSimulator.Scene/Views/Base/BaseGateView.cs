using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using Color = System.Windows.Media.Color;
using Geometry = SharpDX.Direct2D1.Geometry;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;
using RectangleGeometry = SharpDX.Direct2D1.RectangleGeometry;

namespace LogicSimulator.Scene.Views.Base;

public abstract class BaseGateView : SceneObjectView, IStroked
{
    public static readonly IResource GeometryResource = ResourceCache.Register<BaseGateView>((factory, user) => factory.CreateRectangleGeometry(user.Bounds));

    public static readonly IResource FillBrushResource = ResourceCache.Register<BaseGateView>((factory, user) => factory.CreateSolidColorBrush(user.FillColor.ToColor4()));

    public static readonly IResource StrokeBrushResource = ResourceCache.Register<BaseGateView>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

    public const int SelectionPadding = 3;

    #region FillColor

    public Color FillColor
    {
        get => (Color)GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }

    public static readonly DependencyProperty FillColorProperty =
        DependencyProperty.Register(nameof(FillColor), typeof(Color), typeof(BaseGateView), new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFillColorChanged));

    private static void OnFillColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not BaseGateView rectangleView) return;

        rectangleView.ThrowIfDisposed();

        rectangleView.Cache?.Update(rectangleView, FillBrushResource);

        rectangleView.MakeDirty();
    }

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(BaseGateView), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

    private static void OnStrokeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not BaseGateView rectangleView) return;

        rectangleView.ThrowIfDisposed();

        rectangleView.Cache?.Update(rectangleView, StrokeBrushResource);

        rectangleView.MakeDirty();
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => (float)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(BaseGateView), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => (StrokeThicknessType)GetValue(StrokeThicknessTypeProperty);
        set => SetValue(StrokeThicknessTypeProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessTypeProperty =
        DependencyProperty.Register(nameof(StrokeThicknessType), typeof(StrokeThicknessType), typeof(BaseGateView), new FrameworkPropertyMetadata(StrokeThicknessType.Smallest, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    public abstract RectangleF Bounds { get; }

    public override bool HitTest(Vector2 pos, Matrix3x2 worldTransform, float tolerance = 0.25f) =>
        Cache.Get<RectangleGeometry>(this, GeometryResource).FillContainsPoint(pos, TransformMatrix * worldTransform, tolerance);

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 worldTransform, float tolerance = 0.25f) =>
        Cache.Get<RectangleGeometry>(this, GeometryResource).Compare(inputGeometry, Matrix3x2.Invert(TransformMatrix) * worldTransform, tolerance);

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get<SolidColorBrush>(SelectionBrushStaticResource);
        var style = Cache.Get<StrokeStyle>(SelectionStyleStaticResource);

        var bounds = Bounds;
        bounds.Inflate(SelectionPadding, SelectionPadding);

        context.DrawingContext.DrawRectangle(bounds, brush, 1f / scene.Scale, style);
    }
}