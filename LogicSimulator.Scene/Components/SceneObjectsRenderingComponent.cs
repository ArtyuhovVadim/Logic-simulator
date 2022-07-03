using LogicSimulator.Scene.Components.Base;

namespace LogicSimulator.Scene.Components;

public class SceneObjectsRenderingComponent : BaseRenderingComponent
{
    public override void Render(Scene2D scene, Renderer renderer) => renderer.Render(scene, this);
}