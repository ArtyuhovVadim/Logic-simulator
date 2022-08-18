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
        protected set
        {
            _isDragging = value;
            RequireRender();
        }
    }

    [YamlIgnore]
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

    public abstract GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f);
    
    public abstract void Render(Scene2D scene, RenderTarget renderTarget);

    public abstract void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle);
}