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
using RectangleGeometry = SharpDX.Direct2D1.RectangleGeometry;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;

namespace LogicSimulator.Scene.Views;

public class PortView : SceneObjectView, IStroked
{
    public static readonly IStaticResource<StrokeStyle> StrokeStyleResource = ResourceCache.RegisterStatic(factory => factory.CreateStrokeStyle(new StrokeStyleProperties { StartCap = CapStyle.Round, EndCap = CapStyle.Round, LineJoin = LineJoin.Round }));

    public static readonly IResource<PortView, RectangleGeometry> HitTestGeometryResource =
        ResourceCache.Register<PortView, RectangleGeometry>((factory, user) => factory.CreateRectangleGeometry(new RectangleF(0, -user.GetStrokeThickness() / 2f, user.Length, user.GetStrokeThickness())));

    public static readonly IResource<PortView, SolidColorBrush> StrokeBrushResource =
        ResourceCache.Register<PortView, SolidColorBrush>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

    #region Label

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(PortView), new PropertyMetadata(string.Empty, DefaultPropertyChangedHandler));

    #endregion

    #region Length

    public float Length
    {
        get => (float)GetValue(LengthProperty);
        set => SetValue(LengthProperty, value);
    }

    public static readonly DependencyProperty LengthProperty =
        DependencyProperty.Register(nameof(Length), typeof(float), typeof(PortView), new PropertyMetadata(10f, OnGeometryChanged));

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => (float)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(PortView), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => (StrokeThicknessType)GetValue(StrokeThicknessTypeProperty);
        set => SetValue(StrokeThicknessTypeProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessTypeProperty =
        DependencyProperty.Register(nameof(StrokeThicknessType), typeof(StrokeThicknessType), typeof(PortView), new FrameworkPropertyMetadata(StrokeThicknessType.Smallest, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(PortView), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

    private static void OnStrokeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PortView portView) return;

        portView.ThrowIfDisposed();

        portView.Cache?.Update(portView, StrokeBrushResource);

        portView.MakeDirty();
    }

    #endregion

    protected override RectangleF OnWorldBoundsChanged() => Cache?.Get(this, HitTestGeometryResource).GetBounds(WorldTransformMatrix).ToRect() ?? RectangleF.Empty;

    public override bool HitTest(Vector2 pos, Matrix3x2 transform, float tolerance = 0.25f) =>
        Cache.Get(this, HitTestGeometryResource).StrokeContainsPoint(pos, this.GetStrokeThickness(), null, WorldTransformMatrix * transform, tolerance);

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 transform, float tolerance = 0.25f) =>
        Cache.Get(this, HitTestGeometryResource).Compare(inputGeometry, Matrix3x2.Invert(WorldTransformMatrix * transform), tolerance);

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get(this, StrokeBrushResource);
        var strokeThickness = this.GetStrokeThickness(scene);
        context.DrawingContext.DrawLine(Vector2.Zero, new Vector2(Length, 0), brush, strokeThickness, Cache.Get(StrokeStyleResource));
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context) { }

    protected override void OnCacheChanged(ResourceCache cache)
    {
        base.OnCacheChanged(cache);
        Cache.Update(this, HitTestGeometryResource);
        WorldBoundsChanged();
    }

    private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PortView portView) return;

        portView.ThrowIfDisposed();

        portView.Cache?.Update(portView, HitTestGeometryResource);
        portView.WorldBoundsChanged();

        portView.MakeDirty();
    }
}