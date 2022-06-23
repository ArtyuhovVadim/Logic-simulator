namespace LogicSimulator.Scene.Components.Base;

public abstract class BaseRenderingComponent : ResourceDependentObject
{
    public abstract void Render(ComponentRenderer renderer);
}