using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.Utils;
using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class NodeDragTool : BaseTool
{
    private AbstractNode _nodeUnderCursor;
    private EditableSceneObject _nodeUnderCursorOwner;
    private float _snap;

    protected override void OnActivated(Scene2D scene)
    {
        var selectionTool = scene.GetTool<SelectionTool>();

        _nodeUnderCursor = selectionTool.NodeUnderCursor;
        _nodeUnderCursorOwner = selectionTool.NodeUnderCursorOwner;

        _snap = scene.GetComponent<GridRenderingComponent>().CellSize;
    }

    public override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        _nodeUnderCursor.ApplyMove(_nodeUnderCursorOwner, _nodeUnderCursor.UseGridSnap ? pos.Transform(scene.Transform).ApplyGrid(_snap) : pos.Transform(scene.Transform));
    }

    public override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        scene.SwitchTool<SelectionTool>();
    }
}