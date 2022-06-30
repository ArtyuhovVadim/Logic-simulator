using LogicSimulator.Scene.Tools.Base;
using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class SelectionTool : BaseTool
{
    public float Tolerance { get; set; } = 5f;

    public override void MouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {

    }

    public override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        foreach (var sceneObject in scene.Objects)
        {
            if (!sceneObject.IsIntersectsPoint(pos, scene.Transform, Tolerance)) continue;

            if (sceneObject.IsSelected) sceneObject.Unselect();
            else sceneObject.Select();

            break;
        }
    }
}