using LogicSimulator.Shared.Models;
using SharpDX;

namespace LogicSimulator.Scene.Nodes;

public abstract class AbstractNode : IEditableObjectNode
{
    public bool UseGridSnap { get; set; } = true;

    public abstract Vector2 GetLocation(IEditable obj);

    public abstract void ApplyMove(IEditable obj, Vector2 pos);
}