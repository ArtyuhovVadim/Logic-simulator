using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools.Base;
using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class SelectionTool : BaseTool
{
    private IEnumerable<BaseSceneObject> _objectsUnderCursor;

    public event Action SelectionChanged;

    public float SelectionTolerance { get; set; } = 5f;

    public Key MultipleSelectionKey { get; set; } = Key.LeftShift;

    internal override void MouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        if (scene.IsNodeThatIntersectPointExists(pos))
        {
            ToolsController.SwitchTool<NodeDragTool>(tool => tool.MouseLeftButtonDown(scene, pos));
            return;
        }

        _objectsUnderCursor = scene.GetObjectsThatIntersectPoint(pos, SelectionTolerance).Reverse();
    }

    internal override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if (_objectsUnderCursor.Any())
        {
            ToolsController.SwitchTool<DragTool>(tool => tool.MouseLeftButtonDown(scene, pos));
        }
        else
        {
            ToolsController.SwitchTool<RectangleSelectionTool>(tool => tool.MouseLeftButtonDown(scene, pos));
        }
    }

    internal override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        var objects = _objectsUnderCursor.ToList();

        var isMultipleSelectionKeyPressed = Keyboard.IsKeyDown(MultipleSelectionKey);

        if (objects.Count == 0)
        {
            if (!isMultipleSelectionKeyPressed)
            {
                UnselectAllObjects(scene);
                SelectionChanged?.Invoke();
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

            SelectionChanged?.Invoke();
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

            SelectionChanged?.Invoke();
        }
    }

    private void UnselectAllObjects(Scene2D scene)
    {
        foreach (var sceneObject in scene.Objects)
            sceneObject.Unselect();
    }
}