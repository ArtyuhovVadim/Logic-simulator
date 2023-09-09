using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Base;

namespace LogicSimulator.Scene.Layers.Renderers.Base;

public abstract class BaseLayerRenderer<T> : AbstractLayerRenderer where T : BaseSceneLayer
{
    public T Layer { get; private set; } = null!;

    protected override void OnInitialize(BaseSceneLayer layer) => Layer = (T)layer;

    internal override void Render(Scene2D scene, D2DContext context)
    {
        ThrowIfDisposed();

        if (!Layer.IsVisible) return;

        OnRender(scene, context);
    }

    protected abstract void OnRender(Scene2D scene, D2DContext context);

    protected override void Dispose(bool disposingManaged)
    {
        if (disposingManaged)
        {
            Layer.Cache.Release(this);
        }

        base.Dispose(disposingManaged);
    }
}