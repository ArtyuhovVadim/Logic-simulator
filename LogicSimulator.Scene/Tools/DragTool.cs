using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.Utils;
using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class DragTool : BaseTool
{
    private ICollection<BaseSceneObject> _objectsUnderCursor;

    private ICollection<BaseSceneObject> _draggingSceneObjects;

    public float GridSnap { get; set; } = 25f;

    public float DragTolerance { get; set; } = 5f;

    protected override void OnActivated(ToolsController toolsController)
    {
        _draggingSceneObjects = new List<BaseSceneObject>();
    }

    internal override void MouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        _objectsUnderCursor = scene.GetObjectsThatIntersectPoint(pos, DragTolerance).Reverse().ToList();
    }

    internal override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if (!_objectsUnderCursor.Any()) return;

        pos = pos.InvertAndTransform(scene.Transform).ApplyGrid(GridSnap);

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

    internal override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        ToolsController.SwitchToDefaultTool();
    }

    protected override void OnDeactivated(ToolsController toolsController)
    {
        EndDragObjects();
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
        _objectsUnderCursor.Clear();
    }
}