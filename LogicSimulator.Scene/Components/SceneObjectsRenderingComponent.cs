using LogicSimulator.Scene.Components.Base;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class SceneObjectsRenderingComponent : BaseRenderingComponent
{
    public override void Render(Scene2D scene, RenderTarget renderTarget)
    {
        if (!IsVisible) return;

        foreach (var sceneObject in scene.Objects)
        {
            sceneObject.Render(scene, renderTarget);
        }
    }
}