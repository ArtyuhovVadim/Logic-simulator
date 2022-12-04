using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools;
using LogicSimulator.Utils;
using SharpDX;

namespace LogicSimulator.Scene;

public class Scene2D : FrameworkElement
{
    private readonly SceneRenderer _renderer;

    private bool _isLeftMouseButtonPressedOnScene;

    #region Objects

    public IEnumerable<BaseSceneObject> Objects
    {
        get => (IEnumerable<BaseSceneObject>)GetValue(ObjectsProperty);
        set => SetValue(ObjectsProperty, value);
    }

    public static readonly DependencyProperty ObjectsProperty =
        DependencyProperty.Register(nameof(Objects), typeof(IEnumerable<BaseSceneObject>), typeof(Scene2D),
            new PropertyMetadata(Enumerable.Empty<BaseSceneObject>(), OnObjectsChanged));

    private static void OnObjectsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene2D) return;

        if (e.NewValue is not INotifyCollectionChanged collectionChanged) return;

        collectionChanged.CollectionChanged += (_, _) =>
        {
            RenderNotifier.RequestRender(scene2D);
        };
    }

    #endregion

    #region Components

    public IEnumerable<BaseRenderingComponent> Components
    {
        get => (IEnumerable<BaseRenderingComponent>)GetValue(ComponentsProperty);
        set => SetValue(ComponentsProperty, value);
    }

    public static readonly DependencyProperty ComponentsProperty =
        DependencyProperty.Register(nameof(Components), typeof(IEnumerable<BaseRenderingComponent>), typeof(Scene2D),
            new PropertyMetadata(Enumerable.Empty<BaseRenderingComponent>(), OnComponentsChanged));

    private static void OnComponentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene2D) return;

        if (e.NewValue is not INotifyCollectionChanged collectionChanged) return;

        collectionChanged.CollectionChanged += (_, _) =>
        {
            RenderNotifier.RequestRender(scene2D);
        };
    }

    #endregion

    #region ToolsController

    public ToolsController ToolsController
    {
        get => (ToolsController)GetValue(ToolsControllerProperty);
        set => SetValue(ToolsControllerProperty, value);
    }

    public static readonly DependencyProperty ToolsControllerProperty =
        DependencyProperty.Register(nameof(ToolsController), typeof(ToolsController), typeof(Scene2D), new PropertyMetadata(default(ToolsController)));

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
        var renderer = scene._renderer;

        renderer.Transform = renderer.Transform with { M11 = newValue, M22 = newValue };
        renderer.RequestRender();
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

    #region IsRequiredRenderingEnabled

    public bool IsRequiredRenderingEnabled
    {
        get => (bool)GetValue(IsRequiredRenderingEnabledProperty);
        set => SetValue(IsRequiredRenderingEnabledProperty, value);
    }

    public static readonly DependencyProperty IsRequiredRenderingEnabledProperty =
        DependencyProperty.Register(nameof(IsRequiredRenderingEnabled), typeof(bool), typeof(Scene2D), new PropertyMetadata(true));

    #endregion

    public float Dpi => (float)VisualTreeHelper.GetDpi(this).PixelsPerInchX;

    internal ResourceFactory ResourceFactory { get; private set; }

    public Vector2 Size => _renderer.RenderSize;

    public Matrix3x2 Transform => _renderer.Transform;

    public Vector2 Translation
    {
        get => new(_renderer.Transform.M31, _renderer.Transform.M32);
        set
        {
            _renderer.Transform = _renderer.Transform with { M31 = value.X, M32 = value.Y };
            _renderer.RequestRender();
        }
    }

    public Scene2D()
    {
        ClipToBounds = true;
        Focusable = true;
        VisualEdgeMode = EdgeMode.Aliased;

        RenderNotifier.RegisterScene(this);

        _renderer = new SceneRenderer(this);

        ResourceFactory = new ResourceFactory(_renderer);

        Loaded += OnLoaded;
    }

    public T GetComponent<T>() where T : BaseRenderingComponent
    {
        return (T)Components.FirstOrDefault(x => x.GetType() == typeof(T));
    }

    protected override void OnRender(DrawingContext drawingContext) => _renderer.WpfRender(drawingContext);

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) => _renderer.Resize(sizeInfo.NewSize);

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        var pos = GetMousePosition();

        MousePosition = pos.Transform(Transform);

        if (e.LeftButton == MouseButtonState.Pressed && _isLeftMouseButtonPressedOnScene)
            ToolsController?.MouseLeftButtonDragged(this, pos);
        if (e.RightButton == MouseButtonState.Pressed)
            ToolsController?.MouseRightButtonDragged(this, pos);
        if (e.MiddleButton == MouseButtonState.Pressed)
            ToolsController?.MouseMiddleButtonDragged(this, pos);

        ToolsController?.MouseMove(this, pos);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        _isLeftMouseButtonPressedOnScene = true;

        ToolsController?.MouseLeftButtonDown(this, GetMousePosition());
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        if (_isLeftMouseButtonPressedOnScene)
            ToolsController?.MouseLeftButtonUp(this, GetMousePosition());

        _isLeftMouseButtonPressedOnScene = false;

        Mouse.Capture(null);
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonDown(e);

        ToolsController?.MouseRightButtonDown(this, GetMousePosition());
    }

    protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonUp(e);

        ToolsController?.MouseRightButtonUp(this, GetMousePosition());

        Mouse.Capture(null);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);

        if (e.MiddleButton == MouseButtonState.Pressed)
        {
            ToolsController?.MouseMiddleButtonDown(this, GetMousePosition());
        }

        Mouse.Capture(this);
        Keyboard.Focus(this);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);

        if (e.ChangedButton == MouseButton.Middle)
        {
            ToolsController?.MouseMiddleButtonUp(this, GetMousePosition());
            Mouse.Capture(null);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        ToolsController?.KeyDown(this, e);

        if (e.Key == Key.D1)
        {
            ToolsController.SwitchTool<RectanglePlacingTool>();
        }

        if (e.Key == Key.D2)
        {
            ToolsController.SwitchTool<EllipsePlacingTool>();
        }

        if (e.Key == Key.D3)
        {
            ToolsController.SwitchTool<TextBlockPlacingTool>();
        }
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        ToolsController?.KeyUp(this, e);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        ToolsController?.MouseWheel(this, GetMousePosition(), e);
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => _renderer.SetOwnerWindow();

    private Vector2 GetMousePosition() => Mouse.GetPosition(this).ToVector2().DpiCorrect(Dpi);
}