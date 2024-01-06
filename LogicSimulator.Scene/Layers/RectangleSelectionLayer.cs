using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.Layers.Base;
using LogicSimulator.Scene.Layers.Renderers;
using LogicSimulator.Utils;
using SharpDX;
using Color = System.Windows.Media.Color;
using RectangleGeometry = SharpDX.Direct2D1.RectangleGeometry;

namespace LogicSimulator.Scene.Layers;

public class RectangleSelectionLayer : BaseSceneLayer
{
    public static readonly IResource NormalBrushResource =
        ResourceCache.Register<RectangleSelectionLayerRenderer>((factory, user) => factory.CreateSolidColorBrush(user.Layer.NormalColor.ToColor4()));

    public static readonly IResource SecantBrushResource =
        ResourceCache.Register<RectangleSelectionLayerRenderer>((factory, user) => factory.CreateSolidColorBrush(user.Layer.SecantColor.ToColor4()));

    public static readonly IResource RectangleGeometryResource =
        ResourceCache.Register<RectangleSelectionLayerRenderer>((factory, user) => factory.CreateRectangleGeometry(user.Layer.StartPosition, user.Layer.EndPosition));

    #region Geometry

    public RectangleGeometry Geometry
    {
        get => (RectangleGeometry)GetValue(GeometryProperty);
        set => SetValue(GeometryProperty, value);
    }

    public static readonly DependencyProperty GeometryProperty =
        DependencyProperty.Register(nameof(Geometry), typeof(RectangleGeometry), typeof(RectangleSelectionLayer), new PropertyMetadata(default(RectangleGeometry)));

    #endregion

    #region NormalColor

    public Color NormalColor
    {
        get => (Color)GetValue(NormalColorProperty);
        set => SetValue(NormalColorProperty, value);
    }

    public static readonly DependencyProperty NormalColorProperty =
        DependencyProperty.Register(nameof(NormalColor), typeof(Color), typeof(RectangleSelectionLayer), new PropertyMetadata(Colors.Black, OnNormalColorPropertyChanged));

    private static void OnNormalColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RectangleSelectionLayer layer) return;

        layer.Cache?.Update(layer.Renderer, NormalBrushResource);

        layer.MakeDirty();
    }

    #endregion

    #region SecantColor

    public Color SecantColor
    {
        get => (Color)GetValue(SecantColorProperty);
        set => SetValue(SecantColorProperty, value);
    }

    public static readonly DependencyProperty SecantColorProperty =
        DependencyProperty.Register(nameof(SecantColor), typeof(Color), typeof(RectangleSelectionLayer), new PropertyMetadata(Colors.White, OnSecantColorPropertyChanged));

    private static void OnSecantColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RectangleSelectionLayer layer) return;

        layer.Cache?.Update(layer.Renderer, SecantBrushResource);

        layer.MakeDirty();
    }

    #endregion

    #region StartPosition

    public Vector2 StartPosition
    {
        get => (Vector2)GetValue(StartPositionProperty);
        set => SetValue(StartPositionProperty, value);
    }

    public static readonly DependencyProperty StartPositionProperty =
        DependencyProperty.Register(nameof(StartPosition), typeof(Vector2), typeof(RectangleSelectionLayer), new PropertyMetadata(default(Vector2), OnPositionPropertyChanged));

    #endregion

    #region EndPosition

    public Vector2 EndPosition
    {
        get => (Vector2)GetValue(EndPositionProperty);
        set => SetValue(EndPositionProperty, value);
    }

    public static readonly DependencyProperty EndPositionProperty =
        DependencyProperty.Register(nameof(EndPosition), typeof(Vector2), typeof(RectangleSelectionLayer), new PropertyMetadata(default(Vector2), OnPositionPropertyChanged));

    #endregion

    private static void OnPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RectangleSelectionLayer layer) return;

        layer.ThrowIfDisposed();

        layer.Cache?.Update(layer.Renderer, RectangleGeometryResource);

        layer.Geometry = layer.Cache?.Get<RectangleGeometry>(layer.Renderer, RectangleGeometryResource)!;

        layer.MakeDirty();
    }
}