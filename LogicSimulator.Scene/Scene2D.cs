using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;

namespace LogicSimulator.Scene;

public class Scene2D : FrameworkElement
{
    private SceneRenderer _sceneRenderer;

    public Scene2D()
    {
        ClipToBounds = true;
        VisualEdgeMode = EdgeMode.Aliased;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    #region IsRendering

    public bool IsRendering
    {
        get => (bool)GetValue(IsRenderingProperty);
        set => SetValue(IsRenderingProperty, value);
    }

    public static readonly DependencyProperty IsRenderingProperty =
        DependencyProperty.Register(nameof(IsRendering), typeof(bool), typeof(Scene2D), new PropertyMetadata(true, IsRenderingChanged));

    private static void IsRenderingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene2D) return;

        scene2D._sceneRenderer.IsRendering = (bool)e.NewValue;
    }

    #endregion

    #region Objects

    public IEnumerable<BaseSceneObject> Objects
    {
        get => (IEnumerable<BaseSceneObject>)GetValue(ObjectsProperty);
        set => SetValue(ObjectsProperty, value);
    }

    public static readonly DependencyProperty ObjectsProperty =
        DependencyProperty.Register(nameof(Objects), typeof(IEnumerable<BaseSceneObject>), typeof(Scene2D), new PropertyMetadata(default(IEnumerable<BaseSceneObject>)));

    #endregion

    private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(this);

    protected override void OnRender(DrawingContext drawingContext)
    {
        _sceneRenderer.WpfRender(drawingContext, RenderSize);
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        var (width, height) = GetDpiScaledSize();

        _sceneRenderer.Resize(width, height);

        base.OnRenderSizeChanged(sizeInfo);
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        var (width, height) = GetDpiScaledSize();

        _sceneRenderer = new SceneRenderer(width, height);

        Window.GetWindow(this)!.Closed += (_, _) => Utilities.Dispose(ref _sceneRenderer);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (IsInDesignMode || !IsVisible) return;

        CompositionTarget.Rendering += OnRendering;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (IsInDesignMode) return;

        CompositionTarget.Rendering -= OnRendering;
    }

    private void OnRendering(object sender, EventArgs e) => _sceneRenderer.Render(this);

    private (int width, int height) GetDpiScaledSize()
    {
        var dpi = VisualTreeHelper.GetDpi(this).DpiScaleX;
        var width = Math.Max((int)(ActualWidth * dpi), 10);
        var height = Math.Max((int)(ActualHeight * dpi), 10);

        return (width, height);
    }
}