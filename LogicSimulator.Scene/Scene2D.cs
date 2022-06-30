using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.ExtensionMethods;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools;
using LogicSimulator.Scene.Tools.Base;
using SharpDX;

namespace LogicSimulator.Scene;

public class Scene2D : FrameworkElement
{
    private SceneRenderer _sceneRenderer;
    private float dpi;

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
        DependencyProperty.Register(nameof(Objects), typeof(IEnumerable<BaseSceneObject>), typeof(Scene2D), new PropertyMetadata(Enumerable.Empty<BaseSceneObject>()));

    #endregion

    #region RenderingComponents

    public IEnumerable<BaseRenderingComponent> RenderingComponents
    {
        get => (IEnumerable<BaseRenderingComponent>)GetValue(RenderingComponentsProperty);
        set => SetValue(RenderingComponentsProperty, value);
    }

    public static readonly DependencyProperty RenderingComponentsProperty =
        DependencyProperty.Register(nameof(RenderingComponents), typeof(IEnumerable<BaseRenderingComponent>), typeof(Scene2D), new PropertyMetadata(Enumerable.Empty<BaseRenderingComponent>()));

    #endregion

    #region Tools

    public IEnumerable<BaseTool> Tools
    {
        get => (IEnumerable<BaseTool>)GetValue(ToolsProperty);
        set => SetValue(ToolsProperty, value);
    }

    public static readonly DependencyProperty ToolsProperty =
        DependencyProperty.Register(nameof(Tools), typeof(IEnumerable<BaseTool>), typeof(Scene2D), new PropertyMetadata(default(IEnumerable<BaseTool>)));

    #endregion

    public Matrix3x2 Transform => _sceneRenderer.Transform;

    public Scene2D()
    {
        ClipToBounds = true;
        VisualEdgeMode = EdgeMode.Aliased;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public BaseTool CurrentTool { get; private set; }

    private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(this);

    public void SwitchTool<T>() where T : BaseTool
    {
        if (Tools is null)
            throw new ApplicationException("There are no tools in the scene!");

        var tools = Tools.OfType<T>().ToList();

        if (tools.Count != 1)
            throw new ApplicationException($"{typeof(T)} more then one in tools collection!");

        if (tools is null)
            throw new ApplicationException($"{typeof(T)} not contained is tools collection!");

        var tool = tools.First();

        if (CurrentTool is null)
        {
            CurrentTool = tool;
            return;
        }

        CurrentTool?.Deactivate(this);

        CurrentTool = tool;

        CurrentTool.Activate(this);
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        _sceneRenderer.WpfRender(drawingContext, RenderSize);
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        _sceneRenderer.Resize(RenderSize.Width, RenderSize.Height, dpi);

        base.OnRenderSizeChanged(sizeInfo);
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        dpi = (float)VisualTreeHelper.GetDpi(this).PixelsPerInchX;

        _sceneRenderer = new SceneRenderer(RenderSize.Width, RenderSize.Height, dpi);

        Window.GetWindow(this)!.Closed += (_, _) => Utilities.Dispose(ref _sceneRenderer);

        SwitchTool<SelectionTool>();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        var pos = e.GetPosition(this).ToVector2().DpiCorrect(dpi);

        CurrentTool?.MouseMove(this, pos);

        if (e.RightButton == MouseButtonState.Released) return;

        _sceneRenderer.TranslationVector = _rightButtonDownTranslate + pos - _lastRightButtonDownPos;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        var pos = e.GetPosition(this).ToVector2().DpiCorrect(dpi);

        CurrentTool?.MouseLeftButtonDown(this, pos);
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        var pos = e.GetPosition(this).ToVector2().DpiCorrect(dpi);

        CurrentTool?.MouseLeftButtonUp(this, pos);
    }

    private Vector2 _rightButtonDownTranslate;
    private Vector2 _lastRightButtonDownPos;

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonDown(e);

        var pos = e.GetPosition(this).ToVector2().DpiCorrect(dpi);

        _rightButtonDownTranslate = _sceneRenderer.TranslationVector;
        _lastRightButtonDownPos = pos;
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        var pos = e.GetPosition(this).ToVector2().DpiCorrect(dpi);

        var sign = Math.Sign(e.Delta);
        _sceneRenderer.RelativeScale(pos, 0.1f * sign);
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
}