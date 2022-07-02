using SharpDX;

namespace LogicSimulator.Scene.SceneObjects.Base;

public abstract class BaseSceneObject : ResourceDependentObject
{
    private bool _isSelected;
    private bool _isDragging;

    public bool IsDragging
    {
        get => _isDragging;
        protected set
        {
            _isDragging = value;
            RequireRender();
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        protected set
        {
            _isSelected = value;
            RequireRender();
        }
    }

    public abstract void StartDrag(Vector2 pos);

    public abstract void Drag(Vector2 pos);

    public abstract void EndDrag();

    public virtual void Select() => IsSelected = true;

    public virtual void Unselect() => IsSelected = false;

    public abstract bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f);

    public abstract void Render(Renderer renderer);

    public abstract void RenderSelection(Renderer renderer);
}