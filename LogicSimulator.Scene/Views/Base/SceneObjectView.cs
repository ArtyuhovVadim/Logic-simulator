using System.Windows;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using SharpDX;

namespace LogicSimulator.Scene.Views.Base;

public abstract class SceneObjectView : DisposableFrameworkContentElement, IRenderable, IResourceUser, ICacheHost
{
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
        DependencyProperty.Register(nameof(Location), typeof(Vector2), typeof(SceneObjectView), new FrameworkPropertyMetadata(default(Vector2), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region Rotation

    public float Rotation
    {
        get => (float)GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    public static readonly DependencyProperty RotationProperty =
        DependencyProperty.Register(nameof(Rotation), typeof(float), typeof(SceneObjectView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    public bool IsDirty { get; private set; }

    public void Render(Scene2D scene, D2DContext context)
    {
        ThrowIfDisposed();

        OnRender(scene, context);
        IsDirty = false;
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