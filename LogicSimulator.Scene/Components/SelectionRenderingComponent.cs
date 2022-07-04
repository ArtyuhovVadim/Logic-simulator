using LogicSimulator.Scene.Components.Base;

namespace LogicSimulator.Scene.Components;

public class SelectionRenderingComponent : BaseRenderingComponent
{
    public override void Render(Scene2D scene, Renderer renderer)
    {
        if (IsVisible)
        {
            renderer.Render(scene, this);
        }
    }
}