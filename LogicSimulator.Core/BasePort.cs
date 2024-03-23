using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core;

public abstract class BasePort
{
    protected BasePort(BaseGate parent, SignalType initialState = SignalType.Undefined)
    {
        Parent = parent;
        State = initialState;
    }

    public SignalType State { get; protected set; }

    public IEnumerable<Connection> Connections => ConnectionsInternal;

    protected BaseGate Parent { get; }

    protected List<Connection> ConnectionsInternal { get; } = [];

    public abstract void AddConnection(Connection connection);

    public void RemoveConnection(Connection connection) => ConnectionsInternal.Remove(connection);

    public abstract void Invalidate(Simulator simulator, SignalType newState);

    public abstract void RemoveAllConnections();
}