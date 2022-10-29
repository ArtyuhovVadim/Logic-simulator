using SharpDX;
using SharpDX.Direct2D1;
using YamlDotNet.Serialization;

namespace LogicSimulator.Scene.SceneObjects.Base;

public abstract class BaseSceneObject : ResourceDependentObject
{
    private bool _isSelected;
    private bool _isDragging;

    [YamlIgnore]
    public bool IsDragging
    {
        get => _isDragging;
        protected set => SetAndRequestRender(ref _isDragging, value);
    }

    [YamlIgnore]
    public bool IsSelected
    {
        get => _isSelected;
        protected set => SetAndRequestRender(ref _isSelected, value);
    }

    public abstract void StartDrag(Vector2 pos);

    public abstract void Drag(Vector2 pos);

    public abstract void EndDrag();

    public virtual void Select() => IsSelected = true;

    public virtual void Unselect() => IsSelected = false;

    public abstract bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f);

    public abstract GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f);

    public void Render(Scene2D scene, RenderTarget renderTarget)
    {
        Initialize(scene);
        OnRender(scene, renderTarget);
    }

    protected abstract void OnRender(Scene2D scene, RenderTarget renderTarget);

    public abstract void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle);
}