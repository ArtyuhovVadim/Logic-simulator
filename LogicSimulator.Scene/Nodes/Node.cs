using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;

namespace LogicSimulator.Scene.Nodes;

public class Node<T> : AbstractNode where T : EditableSceneObject
{
    private readonly Func<T, Vector2> _getNodePositionFunc;
    private readonly Action<T, Vector2> _applyNodeMoveFunc;

    public Node(Func<T, Vector2> getNodePositionFunc, Action<T, Vector2> applyNodeMoveFunc, bool useGridSnap = true)
    {
        _getNodePositionFunc = getNodePositionFunc;
        _applyNodeMoveFunc = applyNodeMoveFunc;
        UseGridSnap = useGridSnap;
    }

    public Vector2 GetLocation(T obj) => _getNodePositionFunc.Invoke(obj);

    public override Vector2 GetLocation(EditableSceneObject obj) => GetLocation((T)obj);

    public void ApplyMove(T obj, Vector2 pos) => _applyNodeMoveFunc.Invoke(obj, pos);

    public override void ApplyMove(EditableSceneObject obj, Vector2 pos) => ApplyMove((T)obj, pos);
}