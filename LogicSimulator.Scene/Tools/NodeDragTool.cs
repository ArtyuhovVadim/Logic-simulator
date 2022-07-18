using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.ExtensionMethods;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.Scene.Tools.Base;
using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class NodeDragTool : BaseTool
{
    private Node _nodeUnderCursor;
    private float _snap;
    
    protected override void OnActivated(Scene2D scene)
    {
        _nodeUnderCursor = scene.GetTool<SelectionTool>().NodeUnderCursor;
        _snap = scene.GetComponent<GridRenderingComponent>().CellSize;
    }

    public override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        _nodeUnderCursor.ApplyMove(pos.Transform(scene.Transform).ApplyGrid(_snap));
    }

    public override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        _nodeUnderCursor.Unselect();
        scene.SwitchTool<SelectionTool>();
    }
}