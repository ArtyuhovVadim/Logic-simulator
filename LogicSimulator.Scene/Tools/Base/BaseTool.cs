using System.Windows.Input;
using SharpDX;

namespace LogicSimulator.Scene.Tools.Base;

public abstract class BaseTool
{
    public bool IsActive { get; private set; }

    public void Activate(Scene2D scene)
    {
        OnActivated(scene);
        IsActive = true;
    }

    public void Deactivate(Scene2D scene)
    {
        OnDeactivated(scene);
        IsActive = false;
    }

    public virtual void KeyDown(Scene2D scene, KeyEventArgs e) { }

    public virtual void KeyUp(Scene2D scene, KeyEventArgs e) { }

    public virtual void MouseMove(Scene2D scene, Vector2 pos) { }

    public virtual void MouseLeftButtonDown(Scene2D scene, Vector2 pos) { }

    public virtual void MouseLeftButtonDragged(Scene2D scene, Vector2 pos) { }

    public virtual void MouseLeftButtonUp(Scene2D scene, Vector2 pos) { }

    public virtual void MouseRightButtonDown(Scene2D scene, Vector2 pos) { }

    public virtual void MouseRightButtonDragged(Scene2D scene, Vector2 pos) { }

    public virtual void MouseRightButtonUp(Scene2D scene, Vector2 pos) { }

    public virtual void MouseMiddleButtonDragged(Scene2D scene, Vector2 pos) { }

    protected virtual void OnActivated(Scene2D scene) { }

    protected virtual void OnDeactivated(Scene2D scene) { }
}