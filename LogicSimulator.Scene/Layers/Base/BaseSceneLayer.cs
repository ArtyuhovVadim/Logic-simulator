using System.Windows;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Renderers.Base;

namespace LogicSimulator.Scene.Layers.Base;

public abstract class BaseSceneLayer : DisposableFrameworkContentElement, IRenderable, ICacheHost
{
    private bool _isDirty;

    #region IsVisible

    public bool IsVisible
    {
        get => (bool)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    public static readonly DependencyProperty IsVisibleProperty =
        DependencyProperty.Register(nameof(IsVisible), typeof(bool), typeof(BaseSceneLayer), new PropertyMetadata(true, OnIsVisibleChanged));

    private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not BaseSceneLayer layer) return;

        layer.MakeDirty();
    }

    #endregion

    #region Renderer

    public AbstractLayerRenderer Renderer
    {
        get => (AbstractLayerRenderer)GetValue(RendererProperty);
        set => SetValue(RendererProperty, value);
    }

    public static readonly DependencyProperty RendererProperty =
        DependencyProperty.Register(nameof(Renderer), typeof(AbstractLayerRenderer), typeof(BaseSceneLayer), new PropertyMetadata(default(AbstractLayerRenderer), OnRendererChanged));

    private static void OnRendererChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not BaseSceneLayer layer) return;

        layer.ThrowIfDisposed();

        if (e.OldValue is AbstractLayerRenderer oldRender)
        {
            oldRender.Dispose();
        }

        if (e.NewValue is AbstractLayerRenderer newRender)
        {
            newRender.Initialize(layer);
        }

        layer.MakeDirty();
    }

    #endregion

    protected BaseSceneLayer() => MakeDirty();

    public bool IsDirty
    {
        get => _isDirty || OnIsDirtyEvaluation();
        private set => _isDirty = value;
    }

    public ResourceCache Cache { get; private set; } = null!;

    public void InitializeCache(ResourceCache cache)
    {
        ThrowIfDisposed();

        Cache = cache;
        OnCacheChanged(cache);
    }

    public void Render(Scene2D scene, D2DContext context)
    {
        ThrowIfDisposed();

        Renderer?.Render(scene, context);
        IsDirty = false;
    }

    protected void MakeDirty()
    {
        ThrowIfDisposed();
        IsDirty = true;
    }

    protected virtual bool OnIsDirtyEvaluation() => false;

    protected virtual void OnCacheChanged(ResourceCache cache) { }

    protected override void Dispose(bool disposingManaged)
    {
        if (disposingManaged)
        {
            Renderer?.Dispose();
        }

        Cache = null!;

        base.Dispose(disposingManaged);
    }

    protected static void DefaultPropertyChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not BaseSceneLayer layer) return;

        layer.ThrowIfDisposed();

        layer.MakeDirty();
    }
}