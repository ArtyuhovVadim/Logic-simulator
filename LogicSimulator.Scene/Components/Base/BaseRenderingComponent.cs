namespace LogicSimulator.Scene.Components.Base;

public abstract class BaseRenderingComponent : ResourceDependentObject
{
    private bool _isVisible = true;

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            _isVisible = value; 
            RequireRender();
        }
    }

    public abstract void Render(Scene2D scene, Renderer renderer);
}