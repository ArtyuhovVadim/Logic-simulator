using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.Layers.Base;
using LogicSimulator.Scene.Layers.Renderers;
using System.Windows;
using System.Windows.Media;
using LogicSimulator.Utils;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;

namespace LogicSimulator.Scene.Layers;

public class NodesLayer : BaseSceneLayer
{
    public static readonly IResource<NodesLayerRenderer, SolidColorBrush> StrokeBrushResource =
        ResourceCache.Register<NodesLayerRenderer, SolidColorBrush>((factory, user) => factory.CreateSolidColorBrush(user.Layer.StrokeColor.ToColor4()));

    public static readonly IResource<NodesLayerRenderer, SolidColorBrush> FillBrushResource =
        ResourceCache.Register<NodesLayerRenderer, SolidColorBrush>((factory, user) => factory.CreateSolidColorBrush(user.Layer.FillColor.ToColor4()));

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(NodesLayer), new PropertyMetadata(default(Color), OnStrokeColorPropertyChanged));

    private static void OnStrokeColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NodesLayer layer) return;

        layer.ThrowIfDisposed();

        layer.Cache?.Update((NodesLayerRenderer)layer.Renderer, StrokeBrushResource);

        layer.MakeDirty();
    }

    #endregion

    #region FillColor

    public Color FillColor
    {
        get => (Color)GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }

    public static readonly DependencyProperty FillColorProperty =
        DependencyProperty.Register(nameof(FillColor), typeof(Color), typeof(NodesLayer), new PropertyMetadata(default(Color), OnFillColorPropertyChanged));

    private static void OnFillColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NodesLayer layer) return;

        layer.ThrowIfDisposed();

        layer.Cache?.Update((NodesLayerRenderer)layer.Renderer, StrokeBrushResource);

        layer.MakeDirty();
    }

    #endregion

    #region Views

    public IEnumerable<ISelectionRenderable> Views
    {
        get => (IEnumerable<ISelectionRenderable>)GetValue(ViewsProperty);
        set => SetValue(ViewsProperty, value);
    }

    public static readonly DependencyProperty ViewsProperty =
        DependencyProperty.Register(nameof(Views), typeof(IEnumerable<ISelectionRenderable>), typeof(NodesLayer), new PropertyMetadata(default(IEnumerable<ISelectionRenderable>), DefaultPropertyChangedHandler));

    #endregion
}