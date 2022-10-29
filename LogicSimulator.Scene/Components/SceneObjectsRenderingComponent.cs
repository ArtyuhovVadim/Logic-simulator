using LogicSimulator.Scene.Components.Base;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class SceneObjectsRenderingComponent : BaseRenderingComponent
{
    protected override void OnInitialize(Scene2D scene, RenderTarget renderTarget) { }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        if (scene.Objects is null) return;

        foreach (var sceneObject in scene.Objects)
        {
            sceneObject.Render(scene, renderTarget);
        }
    }
}