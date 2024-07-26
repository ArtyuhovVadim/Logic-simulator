using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Shared;
using SharpDX;

namespace LogicSimulator.Scene.Nodes;

public class Node<T> : AbstractNode  where T : EditableSceneObjectView
{
    private readonly Func<T, Vector2> _getNodePositionFunc;
    private readonly Action<T, Vector2> _applyNodeMoveFunc;

    public Node(Func<T, Vector2> getNodePositionFunc, Action<T, Vector2> applyNodeMoveFunc, bool useGridSnap = true)
    {
        _getNodePositionFunc = getNodePositionFunc;
        _applyNodeMoveFunc = applyNodeMoveFunc;
        UseGridSnap = useGridSnap;
    }

    public override Vector2 GetLocation(IEditable obj) => _getNodePositionFunc.Invoke((T)obj);

    public override void ApplyMove(IEditable obj, Vector2 pos) => _applyNodeMoveFunc.Invoke((T)obj, pos);
}