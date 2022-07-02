using SharpDX;

namespace LogicSimulator.Scene.SceneObjects.Base;

public abstract class BaseSceneObject : ResourceDependentObject
{
    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        protected set
        {
            _isSelected = value;
            RequireRender();
        }
    }

    public virtual void Select() => IsSelected = true;

    public virtual void Unselect() => IsSelected = false;

    public abstract bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f);

    public abstract void Render(Renderer renderer);

    public abstract void RenderSelection(Renderer renderer);
}