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
        (_node, _owner) = scene.GetNodeThatIntersectPoint(pos.Transform(scene.Transform));
    }

    internal override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        _node.ApplyMove(_owner, _node.UseGridSnap ? pos.Transform(scene.Transform).ApplyGrid(GridSnap) : pos.Transform(scene.Transform));
    }

    internal override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        ToolsController.SwitchToDefaultTool();
    }
}