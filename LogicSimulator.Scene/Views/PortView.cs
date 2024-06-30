using System.Windows;
using LogicSimulator.Core;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using Color = System.Windows.Media.Color;
using DxColor = SharpDX.Color;

namespace LogicSimulator.Scene.Views;

public class PortView : SceneObjectView, IStroked
{
    public static readonly IStaticResource HighSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(0, 210, 0)));

    public static readonly IStaticResource LowSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(0, 100, 0)));

    public static readonly IStaticResource UndefinedSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(40, 40, 255)));

    public static readonly IStaticResource PosEdgeSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(255, 210, 0)));

    public static readonly IStaticResource NegEdgeSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(255, 100, 0)));

    public static readonly IStaticResource HighImpSignalBrushResource = ResourceCache.RegisterStatic(factory => factory.CreateSolidColorBrush(new DxColor(128, 128, 128)));

    public static readonly IStaticResource StrokeStyleResource = ResourceCache.RegisterStatic(factory => factory.CreateStrokeStyle(new StrokeStyleProperties { StartCap = CapStyle.Round, EndCap = CapStyle.Round, LineJoin = LineJoin.Round }));

    public static readonly IResource HitTestGeometryResource = ResourceCache.Register<PortView>((factory, user) => factory.CreateRectangleGeometry(user.Length, user.GetStrokeThickness()));

    public static readonly IResource StrokeBrushResource = ResourceCache.Register<PortView>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

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
        DependencyProperty.Register(nameof(Length), typeof(float), typeof(PortView), new PropertyMetadata(10f, DefaultPropertyChangedHandler));

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => (float)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(PortView), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => (StrokeThicknessType)GetValue(StrokeThicknessTypeProperty);
        set => SetValue(StrokeThicknessTypeProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessTypeProperty =
        DependencyProperty.Register(nameof(StrokeThicknessType), typeof(StrokeThicknessType), typeof(PortView), new FrameworkPropertyMetadata(StrokeThicknessType.Smallest, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(PortView), new FrameworkPropertyMetadata(System.Windows.Media.Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

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

    public override bool HitTest(Vector2 pos, Matrix3x2 transform, float tolerance = 0.25f) =>
        Cache.Get<RectangleGeometry>(this, HitTestGeometryResource).StrokeContainsPoint(pos, this.GetStrokeThickness(), null, WorldTransformMatrix * transform, tolerance);

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 transform, float tolerance = 0.25f) =>
        Cache.Get<RectangleGeometry>(this, HitTestGeometryResource).Compare(inputGeometry, Matrix3x2.Invert(WorldTransformMatrix * transform), tolerance);

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        //TODO: Рисовать состояние порта только в режиме симуляции
        //var brush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);
        var brush = GetSignalBrush(State);
        var strokeThickness = this.GetStrokeThickness(scene);
        context.DrawingContext.DrawLine(Vector2.Zero, new Vector2(Length, 0), brush, strokeThickness, Cache.Get<StrokeStyle>(StrokeStyleResource));
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context) { }

    protected Brush GetSignalBrush(SignalType type) => type switch
    {
        SignalType.Low => Cache.Get<SolidColorBrush>(LowSignalBrushResource),
        SignalType.High => Cache.Get<SolidColorBrush>(HighSignalBrushResource),
        SignalType.Undefined => Cache.Get<SolidColorBrush>(UndefinedSignalBrushResource),
        SignalType.PosEdge => Cache.Get<SolidColorBrush>(PosEdgeSignalBrushResource),
        SignalType.NegEdge => Cache.Get<SolidColorBrush>(NegEdgeSignalBrushResource),
        SignalType.HighImp => Cache.Get<SolidColorBrush>(HighImpSignalBrushResource),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}