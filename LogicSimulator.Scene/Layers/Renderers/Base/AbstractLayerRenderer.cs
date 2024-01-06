using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Base;

namespace LogicSimulator.Scene.Layers.Renderers.Base;

public abstract class AbstractLayerRenderer : DisposableObject, IResourceUser
{
    public Guid Id { get; } = Guid.NewGuid();

    internal abstract void Render(Scene2D scene, D2DContext context);

    internal void Initialize(BaseSceneLayer layer)
    {
        ThrowIfDisposed();
        OnInitialize(layer);
    }

    protected abstract void OnInitialize(BaseSceneLayer layer);
}