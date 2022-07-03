namespace LogicSimulator.Scene.Components.Base;

public abstract class BaseRenderingComponent : ResourceDependentObject
{
    public abstract void Render(Scene2D scene, Renderer renderer);
}