using LogicSimulator.Core;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;

namespace LogicSimulator.Scene.SceneObjects.Gates.Base;

public abstract class AbstractGateView : BaseSceneObject
{
    public virtual IEnumerable<Vector2> GetPortsPositions()
    {
        return Enumerable.Empty<Vector2>();
    }

    public virtual Port GetPort(int index)
    {
        return null;
    }
}