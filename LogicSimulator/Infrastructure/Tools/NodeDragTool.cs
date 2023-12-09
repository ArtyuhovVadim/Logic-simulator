using System.Windows;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;
using LogicSimulator.Scene.Layers;
using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public class NodeDragTool : BaseTool
{
    private AbstractNode? _node;

    private EditableSceneObjectView? _owner;

    #region GridSnap

    public double GridSnap
    {
        get => (double)GetValue(GridSnapProperty);
        set => SetValue(GridSnapProperty, value);
    }

    public static readonly DependencyProperty GridSnapProperty =
        DependencyProperty.Register(nameof(GridSnap), typeof(double), typeof(NodeDragTool), new PropertyMetadata(25d));

    #endregion

    #region ObjectsLayer

    public ObjectsLayer ObjectsLayer
    {
        get => (ObjectsLayer)GetValue(ObjectsLayerProperty);
        set => SetValue(ObjectsLayerProperty, value);
    }

    public static readonly DependencyProperty ObjectsLayerProperty =
        DependencyProperty.Register(nameof(ObjectsLayer), typeof(ObjectsLayer), typeof(NodeDragTool), new PropertyMetadata(default(ObjectsLayer)));

    #endregion

    protected override void OnKeyDown(Scene2D scene, KeyEventArgs args, Vector2 pos)
    {
        if (args.Key != CancelKey) return;

        ToolsController.SwitchToDefaultTool();
    }

    protected override void OnMouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        foreach (var obj in ObjectsLayer.Objects.Select(ObjectsLayer.GetViewFromItem).OfType<EditableSceneObjectView>().Reverse())
        {
            if (!obj.IsSelected) continue;

            foreach (var node in obj.Nodes)
            {
                if (pos.IsInRectangle(node.GetLocation(obj).RectangleRelativePointAsCenter(AbstractNode.NodeSize / scene.Scale)))
                {
                    _node = node;
                    _owner = obj;
                    return;
                }
            }
        }
    }

    protected override void OnMouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if (_node is null || _owner is null) return;

        pos = _node.UseGridSnap
            ? pos.ApplyGrid((float)GridSnap)
            : pos;

        if (_node.GetLocation(_owner) == pos) return;

        _node.ApplyMove(_owner, pos);
    }

    protected override void OnMouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        if (ActivatedFromOtherTool)
        {
            ToolsController.SwitchToDefaultTool();
        }
        else
        {
            _owner = null;
            _node = null;
        }
    }

    protected override Freezable CreateInstanceCore() => new NodeDragTool();
}