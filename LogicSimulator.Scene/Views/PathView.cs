using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Shared.ExtensionMethods;
using LogicSimulator.Shared.Models;
using SharpDX;
using SharpDX.Direct2D1;
using Color = System.Windows.Media.Color;
using Geometry = SharpDX.Direct2D1.Geometry;
using PathGeometry = SharpDX.Direct2D1.PathGeometry;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;

namespace LogicSimulator.Scene.Views;

public class PathView : SceneObjectView, IStroked
{
    public static readonly IResource<PathView, PathGeometry> GeometryResource =
        ResourceCache.Register<PathView, PathGeometry>((factory, user) => factory.TryParsePathGeometry(user.Geometry));

    public static readonly IResource<PathView, TransformedGeometry> TransformedGeometryResource = ResourceCache.Register<PathView, TransformedGeometry>((factory, user) =>
    {
        var geometry = factory.CreateTransformedGeometry(user.Cache.Get(user, GeometryResource), user.ComputeRenderTransform());
        user._localBounds = geometry.GetBounds().ToRect();
        return geometry;
    });

    public static readonly IResource<PathView, SolidColorBrush> FillBrushResource =
        ResourceCache.Register<PathView, SolidColorBrush>((factory, user) => factory.CreateSolidColorBrush(user.FillColor.ToColor4()));

    public static readonly IResource<PathView, SolidColorBrush> StrokeBrushResource =
        ResourceCache.Register<PathView, SolidColorBrush>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

    private RectangleF _localBounds = RectangleF.Empty;

    #region Geometry

    public string Geometry
    {
        get => (string)GetValue(GeometryProperty);
        set => SetValue(GeometryProperty, value);
    }

    public static readonly DependencyProperty GeometryProperty =
        DependencyProperty.Register(nameof(Geometry), typeof(string), typeof(PathView), new PropertyMetadata(string.Empty, OnTransformedGeometryChanged));

    #endregion

    #region Width

    public float Width
    {
        get => (float)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register(nameof(Width), typeof(float), typeof(PathView), new PropertyMetadata(float.NaN, OnTransformedGeometryChanged));

    #endregion

    #region Height

    public float Height
    {
        get => (float)GetValue(HeightProperty);
        set => SetValue(HeightProperty, value);
    }

    public static readonly DependencyProperty HeightProperty =
        DependencyProperty.Register(nameof(Height), typeof(float), typeof(PathView), new PropertyMetadata(float.NaN, OnTransformedGeometryChanged));

    #endregion

    #region Scale

    public float Scale
    {
        get => (float)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    public static readonly DependencyProperty ScaleProperty =
        DependencyProperty.Register(nameof(Scale), typeof(float), typeof(PathView), new PropertyMetadata(float.NaN, OnTransformedGeometryChanged));

    #endregion

    #region OriginPosition

    public OriginPosition OriginPosition
    {
        get => (OriginPosition)GetValue(OriginPositionProperty);
        set => SetValue(OriginPositionProperty, value);
    }

    public static readonly DependencyProperty OriginPositionProperty =
        DependencyProperty.Register(nameof(OriginPosition), typeof(OriginPosition), typeof(PathView), new PropertyMetadata(OriginPosition.Center, OnTransformedGeometryChanged));

    #endregion

    #region Stretch

    public Stretch Stretch
    {
        get => (Stretch)GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    public static readonly DependencyProperty StretchProperty =
        DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(PathView), new PropertyMetadata(Stretch.None, OnTransformedGeometryChanged));

    #endregion

    #region FillColor

    public Color FillColor
    {
        get => (Color)GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }

    public static readonly DependencyProperty FillColorProperty =
        DependencyProperty.Register(nameof(FillColor), typeof(Color), typeof(PathView), new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFillColorChanged));

    private static void OnFillColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PathView pathView) return;

        pathView.ThrowIfDisposed();

        pathView.Cache?.Update(pathView, FillBrushResource);

        pathView.MakeDirty();
    }

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(PathView), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

    private static void OnStrokeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PathView pathView) return;

        pathView.ThrowIfDisposed();

        pathView.Cache?.Update(pathView, StrokeBrushResource);

        pathView.MakeDirty();
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => (float)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(PathView), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => (StrokeThicknessType)GetValue(StrokeThicknessTypeProperty);
        set => SetValue(StrokeThicknessTypeProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessTypeProperty =
        DependencyProperty.Register(nameof(StrokeThicknessType), typeof(StrokeThicknessType), typeof(PathView), new FrameworkPropertyMetadata(StrokeThicknessType.Smallest, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region SelectionPadding

    public float SelectionPadding
    {
        get => (float)GetValue(SelectionPaddingProperty);
        set => SetValue(SelectionPaddingProperty, value);
    }

    public static readonly DependencyProperty SelectionPaddingProperty =
        DependencyProperty.Register(nameof(SelectionPadding), typeof(float), typeof(PathView), new PropertyMetadata(default(float), DefaultPropertyChangedHandler));

    #endregion

    #region IsAntiAliased

    public bool IsAntiAliased
    {
        get => (bool)GetValue(IsAntiAliasedProperty);
        set => SetValue(IsAntiAliasedProperty, value);
    }

    public static readonly DependencyProperty IsAntiAliasedProperty =
        DependencyProperty.Register(nameof(IsAntiAliased), typeof(bool), typeof(PathView), new PropertyMetadata(true, DefaultPropertyChangedHandler));

    #endregion

    #region IsStroked

    public bool IsStroked
    {
        get => (bool)GetValue(IsStrokedProperty);
        set => SetValue(IsStrokedProperty, value);
    }

    public static readonly DependencyProperty IsStrokedProperty =
        DependencyProperty.Register(nameof(IsStroked), typeof(bool), typeof(PathView), new PropertyMetadata(true, DefaultPropertyChangedHandler));

    #endregion

    #region IsFilled

    public bool IsFilled
    {
        get => (bool)GetValue(IsFilledProperty);
        set => SetValue(IsFilledProperty, value);
    }

    public static readonly DependencyProperty IsFilledProperty =
        DependencyProperty.Register(nameof(IsFilled), typeof(bool), typeof(PathView), new PropertyMetadata(true, DefaultPropertyChangedHandler));

    #endregion

    protected override RectangleF OnWorldBoundsChanged() => _localBounds.Transform(WorldTransformMatrix);

    public override bool HitTest(Vector2 pos, Matrix3x2 transform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get(this, TransformedGeometryResource);
        var matrix = WorldTransformMatrix * transform;

        if (IsFilled)
            return geometry.FillContainsPoint(pos, matrix, tolerance);

        if (IsStroked)
            return geometry.StrokeContainsPoint(pos, this.GetStrokeThickness(), null, matrix, tolerance);

        return geometry.FillContainsPoint(pos, matrix, tolerance);
    }

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 transform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get(this, TransformedGeometryResource);
        var matrix = WorldTransformMatrix * transform;
        return geometry.Compare(inputGeometry, Matrix3x2.Invert(matrix), tolerance);
    }

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var fillBrush = Cache.Get(this, FillBrushResource);
        var strokeBrush = Cache.Get(this, StrokeBrushResource);
        var geometry = Cache.Get(this, TransformedGeometryResource);

        if (IsAntiAliased)
            context.DrawingContext.PushAntialiasMode(AntialiasMode.PerPrimitive);

        if (IsFilled)
            context.DrawingContext.FillGeometry(geometry, fillBrush);

        if (IsStroked)
            context.DrawingContext.DrawGeometry(geometry, strokeBrush, this.GetStrokeThickness(scene));

        if (IsAntiAliased)
            context.DrawingContext.PopAntialiasMode();
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get(SelectionBrushStaticResource);
        var style = Cache.Get(SelectionStyleStaticResource);
        context.DrawingContext.DrawRectangle(_localBounds.ToInflated(SelectionPadding), brush, 1f / scene.Scale, style);
    }

    protected override void OnCacheChanged(ResourceCache cache)
    {
        base.OnCacheChanged(cache);

        Cache.Update(this, StrokeBrushResource);
        Cache.Update(this, FillBrushResource);
        Cache.Update(this, GeometryResource);
        Cache.Update(this, TransformedGeometryResource);
        WorldBoundsChanged();
    }

    private Matrix3x2 ComputeRenderTransform()
    {
        var bounds = Cache.Get(this, GeometryResource).GetBounds().ToRect();
        var result = Matrix3x2.Identity;

        result *= Matrix3x2.Translation(OriginPosition.ToVector2(bounds));

        if (!float.IsNaN(Scale))
            result *= Matrix3x2.Scaling(Scale);

        if (!float.IsNaN(Width) && !float.IsNaN(Height))
            result *= Matrix3x2.Scaling(Stretch.ToScaleVector2(Width, Height, bounds));

        return result;
    }

    private static void OnTransformedGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PathView pathView) return;

        pathView.ThrowIfDisposed();

        pathView.Cache?.Update(pathView, GeometryResource);
        pathView.Cache?.Update(pathView, TransformedGeometryResource);
        pathView.WorldBoundsChanged();

        pathView.MakeDirty();
    }
}