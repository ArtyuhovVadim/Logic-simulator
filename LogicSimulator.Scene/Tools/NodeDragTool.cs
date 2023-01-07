using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.Utils;
using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class NodeDragTool : BaseTool
{
    private AbstractNode _node;

    private EditableSceneObject _owner;

    public float GridSnap { get; set; } = 25f;

    internal override void MouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        (_node, _owner) = scene.GetNodeThatIntersectPoint(pos.InvertAndTransform(scene.Transform));
    }

    internal override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if(_node is null || _owner is null) return;

        pos = _node.UseGridSnap
            ? pos.InvertAndTransform(scene.Transform).ApplyGrid(GridSnap)
            : pos.InvertAndTransform(scene.Transform);

        if (_node.GetLocation(_owner) == pos) return;

        _node.ApplyMove(_owner, pos);
    }

    internal override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        ToolsController.SwitchToDefaultTool();
    }
}