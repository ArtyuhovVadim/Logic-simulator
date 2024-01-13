using System.Windows;
using LogicSimulator.Scene;
using SharpDX;
using Point = System.Windows.Point;

namespace LogicSimulator.Infrastructure.Tools.Base;

public abstract class BaseTool : Freezable
{
    private Point _lastMouseLeftButtonDownPos;
    private Point _lastMouseRightButtonDownPos;

    public event Action<BaseTool>? ContextChanged;

    #region CancelKey

    public Key CancelKey
    {
        get => (Key)GetValue(CancelKeyProperty);
        set => SetValue(CancelKeyProperty, value);
    }

    public static readonly DependencyProperty CancelKeyProperty =
        DependencyProperty.Register(nameof(CancelKey), typeof(Key), typeof(BaseTool), new PropertyMetadata(Key.Escape));

    #endregion

    #region Context

    public object? Context
    {
        get => GetValue(ContextProperty);
        set => SetValue(ContextProperty, value);
    }

    public static readonly DependencyProperty ContextProperty =
        DependencyProperty.Register(nameof(Context), typeof(object), typeof(BaseTool), new PropertyMetadata(default, OnContextPropertyChanged));

    private static void OnContextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not BaseTool tool) return;

        tool.ContextChanged?.Invoke(tool);
    }

    #endregion

    #region MouseLeftButtonDragThreshold

    public float MouseLeftButtonDragThreshold
    {
        get => (float)GetValue(MouseLeftButtonDragThresholdProperty);
        set => SetValue(MouseLeftButtonDragThresholdProperty, value);
    }

    public static readonly DependencyProperty MouseLeftButtonDragThresholdProperty =
        DependencyProperty.Register(nameof(MouseLeftButtonDragThreshold), typeof(float), typeof(BaseTool), new PropertyMetadata(0f));

    #endregion

    #region MouseRightButtonDragThreshold

    public float MouseRightButtonDragThreshold
    {
        get => (float)GetValue(MouseRightButtonDragThresholdProperty);
        set => SetValue(MouseRightButtonDragThresholdProperty, value);
    }

    public static readonly DependencyProperty MouseRightButtonDragThresholdProperty =
        DependencyProperty.Register(nameof(MouseRightButtonDragThreshold), typeof(float), typeof(BaseTool), new PropertyMetadata(0f));

    #endregion

    public bool ActivatedFromOtherTool { get; private set; }

    protected ToolsController ToolsController { get; private set; } = null!;

    public void Activate(ToolsController toolsController, bool activatedFromOtherTool = false)
    {
        ToolsController = toolsController;
        ActivatedFromOtherTool = activatedFromOtherTool;
        OnActivated();
    }

    public void Deactivate()
    {
        OnDeactivated();
        ActivatedFromOtherTool = false;
    }

    public void KeyDown(Scene2D scene, KeyEventArgs args, Vector2 pos) => OnKeyDown(scene, args, pos);

    public void KeyUp(Scene2D scene, KeyEventArgs args, Vector2 pos) => OnKeyUp(scene, args, pos);

    public void MouseMove(Scene2D scene, Vector2 pos) => OnMouseMove(scene, pos);

    public void MouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        _lastMouseLeftButtonDownPos = Mouse.GetPosition(scene);
        OnMouseLeftButtonDown(scene, pos);
    }

    public void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if ((Mouse.GetPosition(scene) - _lastMouseLeftButtonDownPos).Length > MouseLeftButtonDragThreshold)
            OnMouseLeftButtonDragged(scene, pos);
    }

    public void MouseLeftButtonUp(Scene2D scene, Vector2 pos) => OnMouseLeftButtonUp(scene, pos);

    public void MouseRightButtonDown(Scene2D scene, Vector2 pos)
    {
        _lastMouseRightButtonDownPos = Mouse.GetPosition(scene);
        OnMouseRightButtonDown(scene, pos);
    }

    public void MouseRightButtonDragged(Scene2D scene, Vector2 pos)
    {
        if ((Mouse.GetPosition(scene) - _lastMouseRightButtonDownPos).Length > MouseRightButtonDragThreshold)
            OnMouseRightButtonDragged(scene, pos);
    }

    public void MouseRightButtonUp(Scene2D scene, Vector2 pos) => OnMouseRightButtonUp(scene, pos);

    public void MouseMiddleButtonDown(Scene2D scene, Vector2 pos) => OnMouseMiddleButtonDown(scene, pos);

    public void MouseMiddleButtonDragged(Scene2D scene, Vector2 pos) => OnMouseMiddleButtonDragged(scene, pos);

    public void MouseMiddleButtonUp(Scene2D scene, Vector2 pos) => OnMouseMiddleButtonUp(scene, pos);

    public void MouseWheel(Scene2D scene, Vector2 pos, int delta) => OnMouseWheel(scene, pos, delta);

    protected virtual void OnActivated() { }

    protected virtual void OnDeactivated() { }

    protected virtual void OnKeyDown(Scene2D scene, KeyEventArgs args, Vector2 pos) { }

    protected virtual void OnKeyUp(Scene2D scene, KeyEventArgs args, Vector2 pos) { }

    protected virtual void OnMouseMove(Scene2D scene, Vector2 pos) { }

    protected virtual void OnMouseLeftButtonDown(Scene2D scene, Vector2 pos) { }

    protected virtual void OnMouseLeftButtonDragged(Scene2D scene, Vector2 pos) { }

    protected virtual void OnMouseLeftButtonUp(Scene2D scene, Vector2 pos) { }

    protected virtual void OnMouseRightButtonDown(Scene2D scene, Vector2 pos) { }

    protected virtual void OnMouseRightButtonDragged(Scene2D scene, Vector2 pos) { }

    protected virtual void OnMouseRightButtonUp(Scene2D scene, Vector2 pos) { }

    protected virtual void OnMouseMiddleButtonDown(Scene2D scene, Vector2 pos) { }

    protected virtual void OnMouseMiddleButtonDragged(Scene2D scene, Vector2 pos) { }

    protected virtual void OnMouseMiddleButtonUp(Scene2D scene, Vector2 pos) { }

    protected virtual void OnMouseWheel(Scene2D scene, Vector2 pos, int delta) { }
}