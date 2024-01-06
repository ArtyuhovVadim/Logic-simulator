using LogicSimulator.Scene.Views.Base;
using SharpDX;

namespace LogicSimulator.Scene.Nodes;

public abstract class AbstractNode
{
    public static readonly float NodeSize = 4f;

    public bool UseGridSnap { get; set; } = true;

    public abstract Vector2 GetLocation(EditableSceneObjectView obj);

    public abstract void ApplyMove(EditableSceneObjectView obj, Vector2 pos);
}