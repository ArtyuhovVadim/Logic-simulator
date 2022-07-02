namespace LogicSimulator.Scene.Components.Base;

public abstract class BaseComponent : ResourceDependentObject
{
    public abstract void Render(Renderer renderer);
}