using System.Windows;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using SharpDX;

namespace LogicSimulator.Scene.Views.Base;

public abstract class SceneObjectView : DisposableFrameworkContentElement, IRenderable, IResourceUser, ICacheHost
{
    private Matrix3x2 _translateMatrix = Matrix3x2.Identity;
    private Matrix3x2 _rotationMatrix = Matrix3x2.Identity;

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

    //https://stackoverflow.com/a/45392997
    protected Matrix3x2 TransformMatrix => _rotationMatrix * _translateMatrix;

    public bool IsDirty { get; private set; }

    public void Render(Scene2D scene, D2DContext context)
    {
        ThrowIfDisposed();
        context.DrawingContext.PushTransform(TransformMatrix);
        OnRender(scene, context);
        IsDirty = false;
        context.DrawingContext.PopTransform();
    }

    protected abstract void OnRender(Scene2D scene, D2DContext context);

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