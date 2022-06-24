using SharpDX;

namespace LogicSimulator.Scene.SceneObjects.Base;

public abstract class BaseSceneObject : ResourceDependentObject
{
    public bool IsSelected { get; protected set; }

    public virtual void Select() => IsSelected = true;

    public virtual void Unselect() => IsSelected = false;

    public abstract bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f);

    public abstract void Render(ObjectRenderer renderer);
}