namespace LogicSimulator.Scene.SceneObjects.Base;

public abstract class BaseSceneObject : ResourceDependentObject
{
    public abstract void Render(ObjectRenderer objectRenderer);
}