using System.Windows.Input;
using SharpDX;

namespace LogicSimulator.Scene.Tools.Base;

public abstract class BaseTool
{
    public bool CanSwitch { get; protected set; } = true;

    public bool IsActive { get; private set; }

    protected ToolsController ToolsController { get; private set; }

    internal void Activate(ToolsController toolsController)
    {
        IsActive = true;
        ToolsController = toolsController;
        OnActivated(toolsController);
    }

    internal void Deactivate(ToolsController toolsController)
    {
        OnDeactivated(toolsController);
        ToolsController = null;
        IsActive = false;
    }

    protected virtual void OnActivated(ToolsController toolsController) { }

    protected virtual void OnDeactivated(ToolsController toolsController) { }

    internal virtual void KeyDown(Scene2D scene, KeyEventArgs args) { }

    internal virtual void KeyUp(Scene2D scene, KeyEventArgs args) { }

    internal virtual void MouseMove(Scene2D scene, Vector2 pos) { }

    internal virtual void MouseLeftButtonDown(Scene2D scene, Vector2 pos) { }

    internal virtual void MouseLeftButtonDragged(Scene2D scene, Vector2 pos) { }

    internal virtual void MouseLeftButtonUp(Scene2D scene, Vector2 pos) { }

    internal virtual void MouseRightButtonDown(Scene2D scene, Vector2 pos) { }

    internal virtual void MouseRightButtonDragged(Scene2D scene, Vector2 pos) { }

    internal virtual void MouseRightButtonUp(Scene2D scene, Vector2 pos) { }

    internal virtual void MouseMiddleButtonDown(Scene2D scene, Vector2 pos) { }

    internal virtual void MouseMiddleButtonDragged(Scene2D scene, Vector2 pos) { }

    internal virtual void MouseMiddleButtonUp(Scene2D scene, Vector2 pos) { }

    internal virtual void MouseWheel(Scene2D scene, Vector2 pos, int delta) { }
}