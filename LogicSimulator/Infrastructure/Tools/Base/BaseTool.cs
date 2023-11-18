using System.Windows;
using LogicSimulator.Scene;
using SharpDX;

namespace LogicSimulator.Infrastructure.Tools.Base;

public abstract class BaseTool : Freezable
{
    public event Action<BaseTool> ContextChanged;

    #region Context

    public object Context
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

    public bool CanSwitch { get; protected set; } = true;

    public bool ActivatedFromOtherTool { get; private set; }

    protected ToolsController ToolsController { get; private set; }

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

    public void MouseLeftButtonDown(Scene2D scene, Vector2 pos) => OnMouseLeftButtonDown(scene, pos);

    public void MouseLeftButtonDragged(Scene2D scene, Vector2 pos) => OnMouseLeftButtonDragged(scene, pos);

    public void MouseLeftButtonUp(Scene2D scene, Vector2 pos) => OnMouseLeftButtonUp(scene, pos);

    public void MouseRightButtonDown(Scene2D scene, Vector2 pos) => OnMouseRightButtonDown(scene, pos);

    public void MouseRightButtonDragged(Scene2D scene, Vector2 pos) => OnMouseRightButtonDragged(scene, pos);

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