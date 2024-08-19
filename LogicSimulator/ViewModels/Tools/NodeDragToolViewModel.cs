using LogicSimulator.Models.Input;
using LogicSimulator.Shared.ExtensionMethods;
using LogicSimulator.Shared.Models;
using LogicSimulator.ViewModels.Anchorable;

namespace LogicSimulator.ViewModels.Tools;

public class NodeDragToolViewModel : BaseToolViewModel
{
    private readonly SchemeViewModel _scheme;

    private IEditableObjectNode? _node;
    private IEditable? _owner;

    public NodeDragToolViewModel(SchemeViewModel scheme) => _scheme = scheme;

    public float GridStep { get; set; } = 10;

    protected override void OnKeyDown(KeyInputArgs args)
    {
        if (args.Key != CanselKey) return;

        ToolSwitcher.SwitchToDefaultTool(true);
    }

    protected override void OnMouseLeftButtonDown(InputArgs args)
    {
        foreach (var obj in _scheme.HitTester.Objects.OfType<IEditable>().Where(x => x.IsSelected).Reverse())
        {
            foreach (var node in obj.Nodes)
            {
                if (args.Position.IsInRectangle(node.GetLocation(obj).RectangleRelativePointAsCenter(IEditableObjectNode.NodeSize / _scheme.Scale)))
                {
                    _node = node;
                    _owner = obj;
                    return;
                }
            }
        }
    }

    protected override void OnMouseLeftButtonDragged(DragInputArgs args)
    {
        if (_node is null || _owner is null) return;

        var pos = _node.UseGridSnap
            ? args.Position.ApplyGrid(GridStep)
            : args.Position;

        if (_node.GetLocation(_owner) == pos) return;

        _node.ApplyMove(_owner, pos);
    }

    protected override void OnMouseLeftButtonUp(InputArgs args)
    {
        if (ActivatedFromOtherTool)
        {
            ToolSwitcher.SwitchToDefaultTool(true);
        }

        _owner = null;
        _node = null;
    }
}