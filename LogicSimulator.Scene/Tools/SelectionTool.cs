using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.Utils;
using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class SelectionTool : BaseTool
{
    public event Action SelectionChanged;

    public float SelectionTolerance { get; set; } = 5f;

    public Key MultipleSelectionKey { get; set; } = Key.LeftShift;

    public IEnumerable<BaseSceneObject> ObjectsUnderCursor { private set; get; } = Enumerable.Empty<BaseSceneObject>();

    public AbstractNode NodeUnderCursor { private set; get; }

    public EditableSceneObject NodeUnderCursorOwner { private set; get; }

    public override void MouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        var (node, obj) = GetNodeUnderCursor(scene, pos.Transform(scene.Transform));

        if (node is not null)
        {
            NodeUnderCursor = node;
            NodeUnderCursorOwner = obj;
            scene.SwitchTool<NodeDragTool>();
        }

        ObjectsUnderCursor = GetObjectsUnderCursor(scene, pos);
    }

    public override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if (ObjectsUnderCursor.Any())
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
        var objects = ObjectsUnderCursor.ToList();

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

    private IEnumerable<BaseSceneObject> GetObjectsUnderCursor(Scene2D scene, Vector2 pos) =>
        scene.Objects.Where(obj => obj.IsIntersectsPoint(pos, scene.Transform, SelectionTolerance)).Reverse();

    private (AbstractNode nodeUnderCursor, EditableSceneObject nodeUnderCursorOwner) GetNodeUnderCursor(Scene2D scene, Vector2 pos)
    {
        foreach (var sceneObject in scene.Objects.Reverse())
        {
            if (sceneObject is not EditableSceneObject { IsSelected: true } obj) continue;

            foreach (var node in obj.Nodes.Reverse())
            {
                if (pos.IsInRectangle(node.GetLocation(obj).RectangleRelativePointAsCenter(AbstractNode.NodeSize / scene.Scale)))
                    return (node, obj);
            }
        }

        return (null, null);
    }

    private void UnselectAllObjects(Scene2D scene)
    {
        foreach (var sceneObject in scene.Objects)
            sceneObject.Unselect();
    }
}