using SharpDX;

namespace LogicSimulator.Shared;

public interface IEditableObjectNode
{
    static readonly float NodeSize = 4;

    bool UseGridSnap { get; }

    Vector2 GetLocation(IEditable obj);

    void ApplyMove(IEditable obj, Vector2 pos);
}