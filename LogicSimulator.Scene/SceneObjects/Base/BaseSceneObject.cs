using System;
using SharpDX;
using SharpDX.Direct2D1;
using YamlDotNet.Serialization;

namespace LogicSimulator.Scene.SceneObjects.Base;

public abstract class BaseSceneObject : ResourceDependentObject
{
    private bool _isSelected;
    private bool _isDragging;

    private Rotation _rotation = Rotation.Degrees0;
    private Vector2 _location = Vector2.Zero;

    private Matrix3x2 _translateMatrix = Matrix3x2.Identity;
    private Matrix3x2 _rotationMatrix = Matrix3x2.Identity;

    private Vector2 _startDragPosition = Vector2.Zero;
    private Vector2 _startDragLocation;

    [Editable]
    public Vector2 Location
    {
        get => _location;
        set
        {
            _translateMatrix = Matrix3x2.Translation(value);
            SetAndRequestRender(ref _location, value);
        }
    }

    [Editable]
    public Rotation Rotation
    {
        get => _rotation;
        set
        {
            _rotationMatrix = Matrix3x2.Rotation(MathUtil.DegreesToRadians(Utils.RotationToInt(value)), Vector2.Zero);
            SetAndRequestRender(ref _rotation, value);
        }
    }

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

    //https://stackoverflow.com/a/45392997
    protected Matrix3x2 TransformMatrix => _rotationMatrix * _translateMatrix;

    public virtual void StartDrag(Vector2 pos)
    {
        IsDragging = true;

        _startDragLocation = Location;
        _startDragPosition = pos;
    }

    public virtual void Drag(Vector2 pos)
    {
        Location = _startDragLocation - _startDragPosition + pos;
    }

    public virtual void EndDrag()
    {
        IsDragging = false;
    }

    public virtual void Select() => IsSelected = true;

    public virtual void Unselect() => IsSelected = false;

    public abstract bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f);

    public abstract GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f);

    public void Render(Scene2D scene, RenderTarget renderTarget)
    {
        Initialize(scene);

        var tmp = renderTarget.Transform;
        renderTarget.Transform = TransformMatrix * tmp;
        OnRender(scene, renderTarget);
        renderTarget.Transform = tmp;
    }

    public void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var tmp = renderTarget.Transform;
        renderTarget.Transform = TransformMatrix * tmp;
        OnRenderSelection(scene, renderTarget, selectionBrush, selectionStyle);
        renderTarget.Transform = tmp;
    }

    protected abstract void OnRenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle);

    protected abstract void OnRender(Scene2D scene, RenderTarget renderTarget);

    public virtual void Rotate(Vector2 offset)
    {

    }
}