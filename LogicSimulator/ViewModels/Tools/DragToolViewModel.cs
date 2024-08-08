using LogicSimulator.Infrastructure.Tools;
using LogicSimulator.Shared.ExtensionMethods;
using LogicSimulator.Shared.Models;
using LogicSimulator.Shared.Models.HitTest;
using LogicSimulator.ViewModels.Anchorable;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools;

public class DragToolViewModel : BaseToolViewModel
{
    private HitTestResult<IDraggable> _leftButtonDownHitTestResult = null!;
    private readonly List<IDraggable> _draggingSceneObjects = [];
    private readonly SchemeViewModel _scheme;

    public DragToolViewModel(SchemeViewModel scheme) => _scheme = scheme;

    public float GridStep { get; set; } = 10;

    public float DragTolerance { get; set; } = 5;

    protected override void OnKeyDown(KeyInputArgs args)
    {
        if (args.Key != CanselKey) return;

        ToolSwitcher.SwitchToDefaultTool(true);
    }

    protected override void OnMouseLeftButtonDown(InputArgs args)
    {
        _leftButtonDownHitTestResult = _scheme.HitTester.HitTest<IDraggable>(args.Position, DragTolerance);
    }

    protected override void OnMouseLeftButtonDragged(DragInputArgs args)
    {
        if (_leftButtonDownHitTestResult.IsEmpty)
            return;

        var pos = args.Position.ApplyGrid(GridStep);

        var obj = _leftButtonDownHitTestResult.First();

        if (!obj.IsDragging)
        {
            if (obj.IsSelected)
            {
                var objectViews = _scheme.HitTester.Objects.OfType<IDraggable>().Where(x => x.IsSelected).ToList();
                var correctLocation = objectViews.Count == 1;

                foreach (var o in objectViews)
                {
                    StartDragObject(o, pos, correctLocation);
                }
            }
            else
            {
                StartDragObject(obj, pos);
            }
        }

        UpdateDraggingPositions(pos);
    }

    protected override void OnMouseLeftButtonUp(InputArgs args)
    {
        if (ActivatedFromOtherTool)
            ToolSwitcher.SwitchToDefaultTool(true);
        else
            EndDragObjects();
    }

    protected override void OnDeactivated()
    {
        EndDragObjects();
    }

    private void StartDragObject(IDraggable sceneObject, Vector2 pos, bool correctLocation = true)
    {
        if (correctLocation)
            sceneObject.Location = sceneObject.Location.ApplyGrid(GridStep);
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

        _draggingSceneObjects?.Clear();
        _leftButtonDownHitTestResult?.Objects.Clear();
    }
}