using System.Windows;
using System.Windows.Media;
using LogicSimulator.Core;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Shared.ExtensionMethods;
using LogicSimulator.Shared.Models;
using SharpDX;
using SharpDX.Direct2D1;
using Brush = SharpDX.Direct2D1.Brush;
using Color = System.Windows.Media.Color;
using DxColor = SharpDX.Color;
using Geometry = SharpDX.Direct2D1.Geometry;
using RectangleGeometry = SharpDX.Direct2D1.RectangleGeometry;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;

namespace LogicSimulator.Scene.Views;

public class PortView : SceneObjectView, IStroked
{
    public static readonly IStaticResource<SolidColorBrush> HighSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(0, 210, 0)));

    public static readonly IStaticResource<SolidColorBrush> LowSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(0, 100, 0)));

    public static readonly IStaticResource<SolidColorBrush> UndefinedSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(40, 40, 255)));

    public static readonly IStaticResource<SolidColorBrush> PosEdgeSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(255, 210, 0)));

    public static readonly IStaticResource<SolidColorBrush> NegEdgeSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(255, 100, 0)));

    public static readonly IStaticResource<SolidColorBrush> HighImpSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(128, 128, 128)));

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

    #region State

    public SignalType State
    {
        get => (SignalType)GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    public static readonly DependencyProperty StateProperty =
        DependencyProperty.Register(nameof(State), typeof(SignalType), typeof(PortView), new PropertyMetadata(SignalType.Undefined, DefaultPropertyChangedHandler));

    #endregion

    protected override RectangleF OnWorldBoundsChanged() => Cache?.Get(this, HitTestGeometryResource).GetBounds(WorldTransformMatrix).ToRect() ?? RectangleF.Empty;

    public override bool HitTest(Vector2 pos, Matrix3x2 transform, float tolerance = 0.25f) =>
        Cache.Get(this, HitTestGeometryResource).StrokeContainsPoint(pos, this.GetStrokeThickness(), null, WorldTransformMatrix * transform, tolerance);

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 transform, float tolerance = 0.25f) =>
        Cache.Get(this, HitTestGeometryResource).Compare(inputGeometry, Matrix3x2.Invert(WorldTransformMatrix * transform), tolerance);

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        //TODO: Рисовать состояние порта только в режиме симуляции
        //var brush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);
        var brush = GetSignalBrush(State);
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

    private Brush GetSignalBrush(SignalType type) => type switch
    {
        SignalType.Low => Cache.Get(LowSignalBrushResource),
        SignalType.High => Cache.Get(HighSignalBrushResource),
        SignalType.Undefined => Cache.Get(UndefinedSignalBrushResource),
        SignalType.PosEdge => Cache.Get(PosEdgeSignalBrushResource),
        SignalType.NegEdge => Cache.Get(NegEdgeSignalBrushResource),
        SignalType.HighImp => Cache.Get(HighImpSignalBrushResource),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PortView portView) return;

        portView.ThrowIfDisposed();

        portView.Cache?.Update(portView, HitTestGeometryResource);
        portView.WorldBoundsChanged();

        portView.MakeDirty();
    }
}