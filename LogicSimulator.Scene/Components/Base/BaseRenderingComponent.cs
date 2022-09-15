using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components.Base;

public abstract class BaseRenderingComponent : ResourceDependentObject
{
    private bool _isVisible = true;

    public bool IsVisible
    {
        get => _isVisible;
        set => SetAndRequestRender(ref _isVisible, value);
    }

    public void Render(Scene2D scene, RenderTarget renderTarget)
    {
        if (!IsVisible) return;

        OnRender(scene, renderTarget);
    }

    protected abstract void OnRender(Scene2D scene, RenderTarget renderTarget);
}