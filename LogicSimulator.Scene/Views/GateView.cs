using System.Windows;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using LogicSimulator.Scene.Views.Base;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Scene.Views;

public class GateView : SceneObjectView, IStroked
{
    public static readonly IResource GeometryResource = ResourceCache.Register<GateView>((factory, user) => factory.ParsePathGeometry(user.Geometry));

    public static readonly IResource FillBrushResource = ResourceCache.Register<GateView>((factory, user) => factory.CreateSolidColorBrush(user.FillColor.ToColor4()));

    public static readonly IResource StrokeBrushResource = ResourceCache.Register<GateView>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

    #region Width

    public float Width
    {
        get => (float)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register(nameof(Width), typeof(float), typeof(GateView), new PropertyMetadata(default(float), DefaultPropertyChangedHandler));

    #endregion

    #region Height

    public float Height
    {
        get => (float)GetValue(HeightProperty);
        set => SetValue(HeightProperty, value);
    }

    public static readonly DependencyProperty HeightProperty =
        DependencyProperty.Register(nameof(Height), typeof(float), typeof(GateView), new PropertyMetadata(default(float), DefaultPropertyChangedHandler));

    #endregion

    #region Geometry

    public string Geometry
    {
        get => (string)GetValue(GeometryProperty);
        set => SetValue(GeometryProperty, value);
    }

    public static readonly DependencyProperty GeometryProperty =
        DependencyProperty.Register(nameof(Geometry), typeof(string), typeof(GateView), new PropertyMetadata(string.Empty, OnGeometryPropertyChanged));

    private static void OnGeometryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GateView gateView) return;

        gateView.ThrowIfDisposed();

        gateView.Cache?.Update(gateView, GeometryResource);

        gateView.MakeDirty();
    }

    #endregion

    #region FillColor

    public Color FillColor
    {
        get => (Color)GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }

    public static readonly DependencyProperty FillColorProperty =
        DependencyProperty.Register(nameof(FillColor), typeof(Color), typeof(GateView), new FrameworkPropertyMetadata(System.Windows.Media.Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFillColorChanged));

    private static void OnFillColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GateView rectangleView) return;

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
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(GateView), new FrameworkPropertyMetadata(System.Windows.Media.Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

    private static void OnStrokeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GateView rectangleView) return;

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
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(GateView), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => (StrokeThicknessType)GetValue(StrokeThicknessTypeProperty);
        set => SetValue(StrokeThicknessTypeProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessTypeProperty =
        DependencyProperty.Register(nameof(StrokeThicknessType), typeof(StrokeThicknessType), typeof(GateView), new FrameworkPropertyMetadata(StrokeThicknessType.Smallest, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region SelectionPadding

    public float SelectionPadding
    {
        get => (float)GetValue(SelectionPaddingProperty);
        set => SetValue(SelectionPaddingProperty, value);
    }

    public static readonly DependencyProperty SelectionPaddingProperty =
        DependencyProperty.Register(nameof(SelectionPadding), typeof(float), typeof(GateView), new PropertyMetadata(default(float), DefaultPropertyChangedHandler));

    #endregion

    public override bool HitTest(Vector2 pos, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<PathGeometry>(this, GeometryResource);
        var bounds = geometry.GetBounds().ToRect();
        var scaleX = Width / bounds.Width;
        var scaleY = Height / bounds.Height;
        var matrix = Matrix3x2.Scaling(scaleX, scaleY) * Matrix3x2.Translation(-Width / 2, -Height / 2) * TransformMatrix * worldTransform;
        return geometry.FillContainsPoint(pos, matrix, tolerance);
    }

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<PathGeometry>(this, GeometryResource);
        var bounds = geometry.GetBounds().ToRect();
        var scaleX = Width / bounds.Width;
        var scaleY = Height / bounds.Height;
        var matrix = Matrix3x2.Scaling(scaleX, scaleY) * Matrix3x2.Translation(-Width / 2, -Height / 2) * TransformMatrix * worldTransform;
        return geometry.Compare(inputGeometry, Matrix3x2.Invert(matrix), tolerance);
    }

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        if (Width == 0 || Height == 0) return;

        var fillBrush = Cache.Get<SolidColorBrush>(this, FillBrushResource);
        var strokeBrush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);
        var path = Cache.Get<PathGeometry>(this, GeometryResource);

        var bounds = path.GetBounds().ToRect();
        var scaleX = Width / bounds.Width;
        var scaleY = Height / bounds.Height;
        var strokeWidth = this.GetStrokeThickness(scene) / Math.Min(scaleX, scaleY);

        context.DrawingContext.PushAntialiasMode(AntialiasMode.PerPrimitive);
        context.DrawingContext.PushTransform(Matrix3x2.Scaling(scaleX, scaleY) * Matrix3x2.Translation(-Width / 2, -Height / 2));
        context.DrawingContext.FillGeometry(path, fillBrush);
        context.DrawingContext.DrawGeometry(path, strokeBrush, strokeWidth);
        context.DrawingContext.PopTransform();
        context.DrawingContext.PopAntialiasMode();
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get<SolidColorBrush>(SelectionBrushStaticResource);
        var style = Cache.Get<StrokeStyle>(SelectionStyleStaticResource);
        var path = Cache.Get<PathGeometry>(this, GeometryResource);
        var rawBounds = path.GetBounds().ToRect();
        var scaleX = Width / rawBounds.Width;
        var scaleY = Height / rawBounds.Height;
        var bounds = path.GetBounds().ToRect().ScaleAtCenter(scaleX, scaleY);
        bounds.Offset(-rawBounds.Center);
        bounds.Inflate(SelectionPadding, SelectionPadding);

        context.DrawingContext.DrawRectangle(bounds, brush, 1f / scene.Scale, style);
    }
}