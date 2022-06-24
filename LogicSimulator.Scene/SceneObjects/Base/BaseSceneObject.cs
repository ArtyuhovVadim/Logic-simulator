namespace LogicSimulator.Scene.SceneObjects.Base;

public abstract class BaseSceneObject : ResourceDependentObject
{
    public bool IsSelected { get; protected set; }

    public virtual void Select() => IsSelected = true;

    public virtual void Unselect() => IsSelected = false;

    public abstract void Render(ObjectRenderer renderer);
}