using System.Windows;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;
using LogicSimulator.Scene.Layers;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public class DragTool : BaseTool
{
    private ICollection<SceneObjectView> _objectsUnderCursor;

    private ICollection<SceneObjectView> _draggingSceneObjects;

    #region ObjectsLayer

    public ObjectsLayer ObjectsLayer
    {
        get => (ObjectsLayer)GetValue(ObjectsLayerProperty);
        set => SetValue(ObjectsLayerProperty, value);
    }

    public static readonly DependencyProperty ObjectsLayerProperty =
        DependencyProperty.Register(nameof(ObjectsLayer), typeof(ObjectsLayer), typeof(DragTool), new PropertyMetadata(default(ObjectsLayer)));

    #endregion

    #region GridSnap

    public double GridSnap
    {
        get => (double)GetValue(GridSnapProperty);
        set => SetValue(GridSnapProperty, value);
    }

    public static readonly DependencyProperty GridSnapProperty =
        DependencyProperty.Register(nameof(GridSnap), typeof(double), typeof(DragTool), new PropertyMetadata(25d));

    #endregion

    #region DragTolerance

    public double DragTolerance
    {
        get => (double)GetValue(DragToleranceProperty);
        set => SetValue(DragToleranceProperty, value);
    }

    public static readonly DependencyProperty DragToleranceProperty =
        DependencyProperty.Register(nameof(DragTolerance), typeof(double), typeof(DragTool), new PropertyMetadata(5d));

    #endregion

    protected override void OnActivated()
    {
        _draggingSceneObjects = new List<SceneObjectView>();
    }

    protected override void OnKeyDown(Scene2D scene, KeyEventArgs args, Vector2 pos)
    {
        if (args.Key != Key.Escape) return;

        ToolsController.SwitchToDefaultTool();
    }

    protected override void OnMouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        _objectsUnderCursor = ObjectsLayer.Objects
            .Select(ObjectsLayer.GetViewFromItem)
            .Where(objView => objView is not null && objView.HitTest(pos, Matrix3x2.Identity, (float)DragTolerance))
            .Reverse()
            .ToList();
    }

    protected override void OnMouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if (!_objectsUnderCursor.Any()) return;

        pos = pos.ApplyGrid((float)GridSnap);

        var obj = _objectsUnderCursor.First();

        if (!obj.IsDragging)
        {
            if (obj.IsSelected)
            {
                foreach (var o in ObjectsLayer.Objects.Select(ObjectsLayer.GetViewFromItem))
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

    protected override void OnMouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        if (ActivatedFromOtherTool)
            ToolsController.SwitchToDefaultTool();
        else
            EndDragObjects();
    }

    protected override void OnDeactivated()
    {
        EndDragObjects();
    }

    private void StartDragObject(SceneObjectView sceneObject, Vector2 pos)
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

    protected override Freezable CreateInstanceCore() => new DragTool();
}