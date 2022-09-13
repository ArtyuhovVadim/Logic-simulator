using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class SolidClearRenderingComponent : BaseRenderingComponent
{
    public Color4 ClearColor { get; set; } = Color4.White;

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        renderTarget.Clear(ClearColor);
    }
}