using System;
using System.Collections;
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
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene;

public class Scene2D : FrameworkElement
{
    private SceneRenderer _sceneRenderer;
    private SceneTransformController _sceneTransformController;

    #region IsRendering

    public bool IsRendering
    {
        get => (bool) GetValue(IsRenderingProperty);
        set => SetValue(IsRenderingProperty, value);
    }

    public static readonly DependencyProperty IsRenderingProperty =
        DependencyProperty.Register(nameof(IsRendering), typeof(bool), typeof(Scene2D), new PropertyMetadata(true, IsRenderingChanged));

    private static void IsRenderingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene2D) return;

        scene2D._sceneRenderer.IsRendering = (bool) e.NewValue;
    }

    #endregion

    #region Objects

    public IEnumerable<BaseSceneObject> Objects
    {
        get => (IEnumerable<BaseSceneObject>) GetValue(ObjectsProperty);
        set => SetValue(ObjectsProperty, value);
    }

    public static readonly DependencyProperty ObjectsProperty =
        DependencyProperty.Register(nameof(Objects), typeof(IEnumerable<BaseSceneObject>), typeof(Scene2D),
            new PropertyMetadata(Enumerable.Empty<BaseSceneObject>()));

    #endregion

    #region Components

    public IEnumerable<BaseRenderingComponent> Components
    {
        get => (IEnumerable<BaseRenderingComponent>) GetValue(ComponentsProperty);
        set => SetValue(ComponentsProperty, value);
    }

    public static readonly DependencyProperty ComponentsProperty =
        DependencyProperty.Register(nameof(Components), typeof(IEnumerable<BaseRenderingComponent>), typeof(Scene2D),
            new PropertyMetadata(Enumerable.Empty<BaseRenderingComponent>()));

    #endregion

    #region Tools

    public IEnumerable<BaseTool> Tools
    {
        get => (IEnumerable<BaseTool>) GetValue(ToolsProperty);
        set => SetValue(ToolsProperty, value);
    }

    public static readonly DependencyProperty ToolsProperty =
        DependencyProperty.Register(nameof(Tools), typeof(IEnumerable<BaseTool>), typeof(Scene2D), new PropertyMetadata(default(IEnumerable<BaseTool>)));

    #endregion

    public float Dpi { get; private set; }

    internal RenderTarget RenderTarget => _sceneRenderer.RenderTarget;
    
    public Matrix3x2 Transform => _sceneRenderer.Transform;

    public float Scale
    {
        get => _sceneRenderer.Transform.M11;
        set => _sceneRenderer.Transform = _sceneRenderer.Transform with {M11 = value, M22 = value};
    }

    public Vector2 Translation
    {
        get => new(_sceneRenderer.Transform.M31, _sceneRenderer.Transform.M32);
        set => _sceneRenderer.Transform = _sceneRenderer.Transform with {M31 = value.X, M32 = value.Y};
    }

    public Scene2D()
    {
        ClipToBounds = true;
        VisualEdgeMode = EdgeMode.Aliased;
        Focusable = true;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public BaseTool CurrentTool { get; private set; }

    private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(this);

    public T GetComponent<T>() where T : BaseRenderingComponent
    {
        return (T) Components.FirstOrDefault(x => x.GetType() == typeof(T));
    }

    public T GetTool<T>() where T : BaseTool
    {
        return (T) Tools.FirstOrDefault(x => x.GetType() == typeof(T));
    }

    public void SwitchTool<T>() where T : BaseTool
    {
        if (Tools is null)
            throw new ApplicationException("There are no tools in the scene!");

        var tool = Tools.FirstOrDefault(x => x.GetType() == typeof(T));

        if (tool is null) return;

        if (CurrentTool is null)
        {
            CurrentTool = tool;
            return;
        }

        CurrentTool?.Deactivate(this);

        CurrentTool = tool;

        CurrentTool.Activate(this);
    }

    public void RelativeScale(Vector2 pos, float delta, float max = 20f, float min = 0.5f)
    {
        var p = pos.Transform(_sceneRenderer.Transform);

        var newScaleCoefficient = 1 + delta / Scale;
        var newScale = (float) Math.Round(Scale * newScaleCoefficient, 2);

        if (newScale < min || newScale > max) return;

        Translation += p * ((1 - newScaleCoefficient) * Scale);

        Scale = newScale;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        _sceneRenderer.WpfRender(drawingContext, RenderSize);
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        _sceneRenderer.Resize(RenderSize.Width, RenderSize.Height, Dpi);

        base.OnRenderSizeChanged(sizeInfo);
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        Dpi = (float) VisualTreeHelper.GetDpi(this).PixelsPerInchX;

        _sceneRenderer = new SceneRenderer(RenderSize.Width, RenderSize.Height, Dpi);

        _sceneTransformController = new SceneTransformController(this);

        Window.GetWindow(this)!.Closed += (_, _) => Utilities.Dispose(ref _sceneRenderer);

        SwitchTool<SelectionTool>();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        var pos = GetMousePosition();

        if (e.LeftButton == MouseButtonState.Pressed)
            CurrentTool?.MouseLeftButtonDragged(this, pos);
        else if (e.RightButton == MouseButtonState.Pressed)
            CurrentTool?.MouseRightButtonDragged(this, pos);
        else if (e.MiddleButton == MouseButtonState.Pressed)
            CurrentTool?.MouseMiddleButtonDragged(this, pos);
        else
            CurrentTool?.MouseMove(this, pos);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        CurrentTool?.MouseLeftButtonDown(this, GetMousePosition());
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        CurrentTool?.MouseLeftButtonUp(this, GetMousePosition());
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonDown(e);

        CurrentTool?.MouseRightButtonDown(this, GetMousePosition());
    }

    protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonUp(e);

        CurrentTool?.MouseRightButtonUp(this, GetMousePosition());
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);

        Mouse.Capture(this);
        Keyboard.Focus(this);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);

        Mouse.Capture(null);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        CurrentTool?.KeyDown(this, e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        CurrentTool?.KeyUp(this, e);
    }

    protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        base.OnLostKeyboardFocus(e);

        SwitchTool<SelectionTool>();
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

    private Vector2 GetMousePosition() => Mouse.GetPosition(this).ToVector2().DpiCorrect(Dpi);
}