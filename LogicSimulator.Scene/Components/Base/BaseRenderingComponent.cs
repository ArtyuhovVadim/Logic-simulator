using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components.Base;

public abstract class BaseRenderingComponent : ResourceDependentObject
{
    public bool IsVisible { get; set; } = true;

    public abstract void Render(Scene2D scene, RenderTarget renderTarget);
}