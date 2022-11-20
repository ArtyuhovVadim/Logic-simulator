using System.Collections.Generic;
using System.Linq;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Utils;
using SharpDX;

namespace LogicSimulator.Scene;

public static class Scene2DExtensionMethods
{
    public static bool IsObjectThatIntersectPointExists(this Scene2D scene, Vector2 point, float tolerance) =>
        scene.Objects.Any(x => x.IsIntersectsPoint(point, scene.Transform, tolerance));

    public static IEnumerable<BaseSceneObject> GetObjectsThatIntersectPoint(this Scene2D scene, Vector2 point, float tolerance) =>
        scene.Objects.Where(x => x.IsIntersectsPoint(point, scene.Transform, tolerance));

    public static bool IsNodeThatIntersectPointExists(this Scene2D scene, Vector2 point)
    {
        return scene
            .Objects
            .OfType<EditableSceneObject>()
            .Any(obj =>
                obj.Nodes.Any(node => point.IsInRectangle(node.GetLocation(obj).RectangleRelativePointAsCenter(AbstractNode.NodeSize))));
    }

    public static (AbstractNode node, EditableSceneObject owner) GetNodeThatIntersectPoint(this Scene2D scene, Vector2 point)
    {
        foreach (var obj in scene.Objects.OfType<EditableSceneObject>())
        {
            foreach (var node in obj.Nodes)
            {
                if (point.IsInRectangle(node.GetLocation(obj).RectangleRelativePointAsCenter(AbstractNode.NodeSize)))
                {
                    return (node, obj);
                }
            }
        }

        return (null, null);
    }
}