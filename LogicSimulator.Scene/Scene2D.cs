using System;
using System.Collections.Generic;
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

    private bool _isLeftMouseButtonPressedOnScene;

    #region Objects

    public IEnumerable<BaseSceneObject> Objects
    {
        get => (IEnumerable<BaseSceneObject>)GetValue(ObjectsProperty);
        set => SetValue(ObjectsProperty, value);
    }

    public static readonly DependencyProperty ObjectsProperty =
        DependencyProperty.Register(nameof(Objects), typeof(IEnumerable<BaseSceneObject>), typeof(Scene2D),
            new PropertyMetadata(Enumerable.Empty<BaseSceneObject>()));

    #endregion

    #region Components

    public IEnumerable<BaseRenderingComponent> Components
    {
        get => (IEnumerable<BaseRenderingComponent>)GetValue(ComponentsProperty);
        set => SetValue(ComponentsProperty, value);
    }

    public static readonly DependencyProperty ComponentsProperty =
        DependencyProperty.Register(nameof(Components), typeof(IEnumerable<BaseRenderingComponent>), typeof(Scene2D),
            new PropertyMetadata(Enumerable.Empty<BaseRenderingComponent>()));

    #endregion

    #region Tools

    public IEnumerable<BaseTool> Tools
    {
        get => (IEnumerable<BaseTool>)GetValue(ToolsProperty);
        set => SetValue(ToolsProperty, value);
    }

    public static readonly DependencyProperty ToolsProperty =
        DependencyProperty.Register(nameof(Tools), typeof(IEnumerable<BaseTool>), typeof(Scene2D),
            new PropertyMetadata(Enumerable.Empty<BaseTool>(), OnToolsChanged));

    private static void OnToolsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene) return;

        scene.CurrentTool = ((IEnumerable<BaseTool>)e.NewValue).First();
    }

    #endregion

    #region Scale

    public float Scale
    {
        get => (float)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    public static readonly DependencyProperty ScaleProperty =
        DependencyProperty.Register(nameof(Scale), typeof(float), typeof(Scene2D), new PropertyMetadata(1f, OnScaleChanged));

    private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene) return;

        var newValue = (float)e.NewValue;
        var render = scene._sceneRenderer;

        render.Transform = render.Transform with { M11 = newValue, M22 = newValue };
    }

    #endregion

    #region MousePosition

    private static readonly DependencyPropertyKey MousePositionPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(MousePosition), typeof(Vector2), typeof(Scene2D), new PropertyMetadata(Vector2.Zero));

    public static readonly DependencyProperty MousePositionProperty
        = MousePositionPropertyKey.DependencyProperty;

    public Vector2 MousePosition
    {
        get => (Vector2)GetValue(MousePositionProperty);
        private set => SetValue(MousePositionPropertyKey, value);
    }

    #endregion

    public float Dpi => (float)VisualTreeHelper.GetDpi(this).PixelsPerInchX;

    //TODO: Костыль!
    internal RenderTarget RenderTarget => _sceneRenderer.RenderTarget;

    public Matrix3x2 Transform => _sceneRenderer.Transform;

    public Vector2 Translation
    {
        get => new(_sceneRenderer.Transform.M31, _sceneRenderer.Transform.M32);
        set => _sceneRenderer.Transform = _sceneRenderer.Transform with { M31 = value.X, M32 = value.Y };
    }

    public Scene2D()
    {
        ClipToBounds = true;
        Focusable = true;
        VisualEdgeMode = EdgeMode.Aliased;

        _sceneTransformController = new SceneTransformController(this);

        Loaded += OnLoaded;
    }

    public BaseTool CurrentTool { get; private set; }

    public T GetComponent<T>() where T : BaseRenderingComponent
    {
        return (T)Components.FirstOrDefault(x => x.GetType() == typeof(T));
    }

    public T GetTool<T>() where T : BaseTool
    {
        return (T)Tools.FirstOrDefault(x => x.GetType() == typeof(T));
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

    protected override void OnRender(DrawingContext drawingContext)
    {
        _sceneRenderer.WpfRender(drawingContext, RenderSize);
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        _sceneRenderer.Resize(RenderSize);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        var pos = GetMousePosition();

        MousePosition = pos.Transform(Transform);

        if (e.LeftButton == MouseButtonState.Pressed && _isLeftMouseButtonPressedOnScene)
            CurrentTool?.MouseLeftButtonDragged(this, pos);
        else if (e.RightButton == MouseButtonState.Pressed && _isLeftMouseButtonPressedOnScene)
            CurrentTool?.MouseRightButtonDragged(this, pos);
        else if (e.MiddleButton == MouseButtonState.Pressed && _isLeftMouseButtonPressedOnScene)
            CurrentTool?.MouseMiddleButtonDragged(this, pos);
        else
            CurrentTool?.MouseMove(this, pos);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        _isLeftMouseButtonPressedOnScene = true;

        CurrentTool?.MouseLeftButtonDown(this, GetMousePosition());
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        if (_isLeftMouseButtonPressedOnScene)
            CurrentTool?.MouseLeftButtonUp(this, GetMousePosition());

        _isLeftMouseButtonPressedOnScene = false;

        Mouse.Capture(null);
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

        Mouse.Capture(null);
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

        if (e.ChangedButton == MouseButton.Middle)
        {
            Mouse.Capture(null);
        }
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
        if (_sceneRenderer is null)
        {
            _sceneRenderer = new SceneRenderer(this);
            CompositionTarget.Rendering += (_, _) => _sceneRenderer.RequestRender();
        }
    }

    private Vector2 GetMousePosition() => Mouse.GetPosition(this).ToVector2().DpiCorrect(Dpi);
}