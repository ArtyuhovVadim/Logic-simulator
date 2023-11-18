using System.Windows;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Views.Base;

public abstract class SceneObjectView : DisposableFrameworkContentElement, ISelectionRenderable, IResourceUser, ICacheHost
{
    private Matrix3x2 _translateMatrix = Matrix3x2.Identity;
    private Matrix3x2 _rotationMatrix = Matrix3x2.Identity;

    private Vector2 _startDragPosition = Vector2.Zero;
    private Vector2 _startDragLocation = Vector2.Zero;

    protected SceneObjectView() => MakeDirty();

    public ResourceCache Cache { get; private set; } = null!;

    public Guid Id { get; } = Guid.NewGuid();

    #region Location

    public Vector2 Location
    {
        get => (Vector2)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    public static readonly DependencyProperty LocationProperty =
        DependencyProperty.Register(nameof(Location), typeof(Vector2), typeof(SceneObjectView), new FrameworkPropertyMetadata(default(Vector2), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLocationChanged));

    private static void OnLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not SceneObjectView view) return;

        view.ThrowIfDisposed();

        view._translateMatrix = Matrix3x2.Translation((Vector2)e.NewValue);

        view.MakeDirty();
    }

    #endregion

    #region Rotation

    public float Rotation
    {
        get => (float)GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    public static readonly DependencyProperty RotationProperty =
        DependencyProperty.Register(nameof(Rotation), typeof(float), typeof(SceneObjectView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRotationChanged));

    private static void OnRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not SceneObjectView view) return;

        view.ThrowIfDisposed();

        view._rotationMatrix = Matrix3x2.Rotation(MathUtil.DegreesToRadians((float)e.NewValue), Vector2.Zero);

        view.MakeDirty();
    }

    #endregion

    #region IsSelected

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(SceneObjectView), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region IsDragging

    private static readonly DependencyPropertyKey IsDraggingPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(IsDragging), typeof(bool), typeof(SceneObjectView), new FrameworkPropertyMetadata(default(bool)));

    public bool IsDragging => (bool)GetValue(IsDraggingPropertyKey.DependencyProperty);

    #endregion

    //https://stackoverflow.com/a/45392997
    public Matrix3x2 TransformMatrix => _rotationMatrix * _translateMatrix;

    public bool IsDirty { get; private set; }

    public void Select()
    {
        IsSelected = true;
        MakeDirty();
    }

    public void Unselect()
    {
        IsSelected = false;
        MakeDirty();
    }

    public virtual void StartDrag(Vector2 pos)
    {
        SetValue(IsDraggingPropertyKey, true);

        _startDragLocation = Location;
        _startDragPosition = pos;
    }

    public virtual void Drag(Vector2 pos)
    {
        Location = _startDragLocation - _startDragPosition + pos;
    }

    public virtual void EndDrag()
    {
        SetValue(IsDraggingPropertyKey, false);
    }

    public Vector2 WorldToLocalSpace(Vector2 worldPos) => worldPos.InvertAndTransform(TransformMatrix);

    public Vector2 LocalToWorldSpace(Vector2 localPos) => localPos.Transform(TransformMatrix);

    public bool HitTest(Vector2 pos, float tolerance = 0.25f) =>
        HitTest(pos, Matrix3x2.Identity, tolerance);

    public abstract bool HitTest(Vector2 pos, Matrix3x2 worldTransform, float tolerance = 0.25f);

    public GeometryRelation HitTest(Geometry inputGeometry, float tolerance = 0.25f) =>
        HitTest(inputGeometry, Matrix3x2.Identity, tolerance);

    public abstract GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 worldTransform, float tolerance = 0.25f);

    public void Render(Scene2D scene, D2DContext context)
    {
        ThrowIfDisposed();
        context.DrawingContext.PushTransform(TransformMatrix);
        OnRender(scene, context);
        IsDirty = false;
        context.DrawingContext.PopTransform();
    }

    public void RenderSelection(Scene2D scene, D2DContext context)
    {
        ThrowIfDisposed();
        context.DrawingContext.PushTransform(TransformMatrix);
        OnRenderSelection(scene, context);
        IsDirty = false;
        context.DrawingContext.PopTransform();
    }

    protected abstract void OnRender(Scene2D scene, D2DContext context);

    protected abstract void OnRenderSelection(Scene2D scene, D2DContext context);

    public void InitializeCache(ResourceCache cache)
    {
        ThrowIfDisposed();
        Cache = cache;
    }

    protected void MakeDirty()
    {
        ThrowIfDisposed();
        IsDirty = true;
    }

    protected override void Dispose(bool disposingManaged)
    {
        if (disposingManaged)
        {
            Cache.Release(this);
        }

        Cache = null!;

        base.Dispose(disposingManaged);
    }

    protected static void DefaultPropertyChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not SceneObjectView obj) return;

        obj.ThrowIfDisposed();

        obj.MakeDirty();
    }
}