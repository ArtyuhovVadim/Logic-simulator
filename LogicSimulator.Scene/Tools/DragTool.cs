using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.ExtensionMethods;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools.Base;
using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class DragTool : BaseTool
{
    private List<BaseSceneObject> _objectsUnderCursor;

    private readonly List<BaseSceneObject> _draggingSceneObjects = new();

    private float _snap = 0f;

    protected override void OnActivated(Scene2D scene)
    {
        _objectsUnderCursor = scene.GetTool<SelectionTool>().ObjectUnderCursor.ToList();
        _snap = scene.GetComponent<GridComponent>().CellSize;
    }

    public override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if (!_objectsUnderCursor.Any()) return;

        pos = pos.Transform(scene.Transform).ApplyGrid(_snap);

        var obj = _objectsUnderCursor.First();

        if (!obj.IsDragging)
        {
            if (obj.IsSelected)
            {
                foreach (var o in scene.Objects)
                {
                    if (o.IsSelected)
                    {
                        StartDragObject(o, pos);
                    }
                }
            }
            else
            {
                StartDragObject(obj, pos);
            }
        }

        UpdateDraggingPositions(pos);
    }

    public override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        EndDragObjects();

        scene.SwitchTool<SelectionTool>();
    }

    private void StartDragObject(BaseSceneObject sceneObject, Vector2 pos)
    {
        sceneObject.StartDrag(pos);
        _draggingSceneObjects.Add(sceneObject);
    }

    private void UpdateDraggingPositions(Vector2 pos)
    {
        foreach (var o in _draggingSceneObjects)
        {
            o.Drag(pos);
        }
    }

    private void EndDragObjects()
    {
        foreach (var o in _draggingSceneObjects)
        {
            o.EndDrag();
        }

        _draggingSceneObjects.Clear();
    }
}