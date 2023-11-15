using System.Windows;
using LogicSimulator.Scene;
using SharpDX;

namespace LogicSimulator.Infrastructure.Tools.Base;

public abstract class BaseTool : Freezable
{
    private bool _isActive;
    public bool CanSwitch { get; protected set; } = true;

    // public bool IsActive
    // {
    //     get => _isActive;
    //     set => _isActive = value;
    // }

    #region IsActive

    private static readonly DependencyPropertyKey IsActivePropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(IsActive), typeof(bool), typeof(BaseTool), new PropertyMetadata(default(bool)));

    public bool IsActive
    {
        get => (bool)GetValue(IsActivePropertyKey.DependencyProperty);
        private set => SetValue(IsActivePropertyKey, value);
    }

    #endregion

    protected ToolsController ToolsController { get; private set; }

    public void Activate(ToolsController toolsController)
    {
        IsActive = true;
        ToolsController = toolsController;
        OnActivated();
    }

    public void Deactivate()
    {
        OnDeactivated();
        IsActive = false;
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