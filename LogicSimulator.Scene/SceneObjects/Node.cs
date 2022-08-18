using System;
using SharpDX;

namespace LogicSimulator.Scene.SceneObjects;

public class Node
{
    private readonly Func<Vector2> _getNodePositionFunc;
    private readonly Action<Vector2> _applyNodeMoveFunc;

    public static readonly float NodeSize = 4f;

    public Node(Func<Vector2> getNodePositionFunc, Action<Vector2> applyNodeMoveFunc)
    {
        _getNodePositionFunc = getNodePositionFunc;
        _applyNodeMoveFunc = applyNodeMoveFunc;
    }

    public bool IsSelected { get; private set; }

    public Vector2 Location => _getNodePositionFunc.Invoke();

    public void ApplyMove(Vector2 pos) => _applyNodeMoveFunc.Invoke(pos);

    public void Select() => IsSelected = true;

    public void Unselect() => IsSelected = false;
}