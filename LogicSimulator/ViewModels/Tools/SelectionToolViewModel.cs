using LogicSimulator.Infrastructure.Tools;
using LogicSimulator.Shared.ExtensionMethods;
using LogicSimulator.Shared.Models;
using LogicSimulator.Shared.Models.HitTest;
using LogicSimulator.ViewModels.Anchorable;

namespace LogicSimulator.ViewModels.Tools;

public class SelectionToolViewModel : BaseToolViewModel
{
    private HitTestResult<ISelectable> _leftButtonDownHitTestResult = null!;
    private readonly SchemeViewModel _scheme;

    public SelectionToolViewModel(SchemeViewModel scheme) => _scheme = scheme;

    public event Action? SelectedObjectsChanged;

    public float SelectionThreshold { get; set; } = 5;

    public float DragThreshold { get; set; } = 10;

    public Key MultipleSelectionKey { get; set; } = Key.LeftShift;

    protected override void OnMouseLeftButtonDown(InputArgs args)
    {
        var isNodeThatIntersectPointExists =
            _scheme.HitTester.Objects.OfType<IEditable>()
                .Where(x => x.IsSelected)
                .Any(obj => obj.Nodes.Any(node => args.Position.IsInRectangle(node.GetLocation(obj).RectangleRelativePointAsCenter(IEditableObjectNode.NodeSize / _scheme.Scale))));

        if (isNodeThatIntersectPointExists)
        {
            ToolSwitcher.SwitchTool<NodeDragToolViewModel>(true, tool => tool.MouseLeftButtonDown(args));
            return;
        }

        _leftButtonDownHitTestResult = _scheme.HitTester.HitTest<ISelectable>(args.Position, SelectionThreshold);
    }

    protected override void OnMouseLeftButtonDragged(DragInputArgs args)
    {
        if (!_leftButtonDownHitTestResult.IsEmpty)
        {
            if (args.Delta.Length() > DragThreshold)
                ToolSwitcher.SwitchTool<DragToolViewModel>(true, tool => tool.MouseLeftButtonDown(args with { Position = args.StartPosition }));
        }
        else
        {
            ToolSwitcher.SwitchTool<RectangleSelectionToolViewModel>(true, tool => tool.MouseLeftButtonDown(args));
        }
    }

    protected override void OnMouseLeftButtonUp(InputArgs args)
    {
        var objects = _leftButtonDownHitTestResult.Objects;

        var isMultipleSelectionKeyPressed = Keyboard.IsKeyDown(MultipleSelectionKey);

        if (objects.Count == 0)
        {
            if (!isMultipleSelectionKeyPressed)
            {
                UnselectAllObjects();
                SelectedObjectsChanged?.Invoke();
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
                    UnselectAllObjects();
                }

                obj.Select();
            }

            SelectedObjectsChanged?.Invoke();
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
                UnselectAllObjects();
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

            SelectedObjectsChanged?.Invoke();
        }
    }

    private void UnselectAllObjects()
    {
        foreach (var obj in _scheme.Objects)
        {
            obj.IsSelected = false;
        }
    }
}