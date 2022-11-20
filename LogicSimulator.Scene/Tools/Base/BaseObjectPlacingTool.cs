using System.Collections.Generic;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Utils;
using SharpDX;

namespace LogicSimulator.Scene.Tools.Base;

public abstract class BaseObjectPlacingTool<T> : BaseTool where T : BaseSceneObject, new()
{
    public float GridSnap { get; set; } = 25f;

    protected T PlacingObject { get; private set; }

    protected List<PlacingStep> PlacingSteps { get; private set; } = new();

    private bool _isMouseRightButtonDragged;

    private bool _isStarted;

    private int _placingProgress;

    internal override void MouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        if (!_isStarted)
        {
            StartObjectPlacing(scene);
            _isStarted = true;
        }

        PlacingSteps[_placingProgress].Start(scene, PlacingSteps[_placingProgress].UseGrid ?
            pos.Transform(scene.Transform).ApplyGrid(GridSnap) : pos.Transform(scene.Transform));

        _placingProgress++;

        if (_placingProgress >= PlacingSteps.Count)
        {
            scene.UnselectAllObjects();
            PlacingObject.Select();
            ToolsController.OnSelectedObjectsChanged();
            EndObjectPlacing();
        }
    }

    internal override void MouseRightButtonDragged(Scene2D scene, Vector2 pos)
    {
        _isMouseRightButtonDragged = true;
    }

    internal override void MouseRightButtonUp(Scene2D scene, Vector2 pos)
    {
        if (_isMouseRightButtonDragged)
        {
            _isMouseRightButtonDragged = false;
            return;
        }

        if (_placingProgress == 0)
        {
            EndObjectPlacing();
            ToolsController.SwitchToDefaultTool();
        }
        else
        {
            CancelObjectPlacing(scene);
        }
    }

    internal override void MouseMove(Scene2D scene, Vector2 pos)
    {
        if (_isStarted)
        {
            PlacingSteps[_placingProgress].Start(scene, PlacingSteps[_placingProgress].UseGrid ?
                pos.Transform(scene.Transform).ApplyGrid(GridSnap) : pos.Transform(scene.Transform));
        }
    }

    private void StartObjectPlacing(Scene2D scene)
    {
        if (scene.Objects is not ICollection<BaseSceneObject> objects) return;
        CanSwitch = false;
        PlacingObject = new T();
        objects.Add(PlacingObject);
    }

    private void EndObjectPlacing()
    {
        _placingProgress = 0;
        PlacingObject = null;
        _isStarted = false;

        CanSwitch = true;
    }

    private void CancelObjectPlacing(Scene2D scene)
    {
        if (scene.Objects is not ICollection<BaseSceneObject> objects) return;

        objects.Remove(PlacingObject);
        EndObjectPlacing();
    }
}