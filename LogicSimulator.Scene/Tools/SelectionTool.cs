using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools.Base;
using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class SelectionTool : BaseTool
{
    public float SelectionTolerance { get; set; } = 5f;

    public Key MultipleSelectionKey { get; set; } = Key.LeftShift;

    public IEnumerable<BaseSceneObject> ObjectUnderCursor { private set; get; } = Enumerable.Empty<BaseSceneObject>();

    public override void MouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        ObjectUnderCursor = GetObjectsUnderCursor(scene, pos);
    }

    public override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if (ObjectUnderCursor.Any())
        {
            scene.SwitchTool<DragTool>();
        }
        else
        {
            scene.SwitchTool<RectangleSelectionTool>();
        }
    }

    public override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        var objects = ObjectUnderCursor.ToList();

        var isMultipleSelectionKeyPressed = Keyboard.IsKeyDown(MultipleSelectionKey);

        if (objects.Count == 0)
        {
            if (!isMultipleSelectionKeyPressed)
            {
                UnselectAllObjects(scene);
            }
        }
        else if (objects.Count == 1)
        {
            var obj = objects.First();

            if (obj.IsSelected) obj.Unselect();
            else
            {
                if (!isMultipleSelectionKeyPressed)
                {
                    UnselectAllObjects(scene);
                }

                obj.Select();
            }
        }
        else
        {
            var selectedObjectIndex = -1;

            for (var i = 0; i < objects.Count; i++)
            {
                if (objects[i].IsSelected)
                {
                    selectedObjectIndex = i;
                    break;
                }
            }

            if (!isMultipleSelectionKeyPressed)
            {
                UnselectAllObjects(scene);
            }

            if (selectedObjectIndex == -1)
            {
                objects[0].Select();
            }
            else
            {
                var nextSelectedObjectIndex = selectedObjectIndex + 1 < objects.Count ? selectedObjectIndex + 1 : 0;

                objects[selectedObjectIndex].Unselect();
                objects[nextSelectedObjectIndex].Select();
            }
        }
    }

    private IEnumerable<BaseSceneObject> GetObjectsUnderCursor(Scene2D scene, Vector2 pos) =>
        scene.Objects.Where(obj => obj.IsIntersectsPoint(pos, scene.Transform, SelectionTolerance)).Reverse();

    private void UnselectAllObjects(Scene2D scene)
    {
        foreach (var sceneObject in scene.Objects)
            sceneObject.Unselect();
    }
}