using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core;

public class OutputPort : BasePort
{
    public OutputPort(BaseGate parent, SignalType initialState = SignalType.Undefined) : base(parent, initialState) { }

    public override void AddConnection(Connection connection)
    {
        ConnectionsInternal.Add(connection);
    }

    public override void Invalidate(Simulator simulator, SignalType newState)
    {
        if (State == newState)
            return;

        State = newState;

        foreach (var connection in ConnectionsInternal)
        {
            connection.Invalidate(simulator);
        }
    }

    public override void RemoveAllConnections()
    {
        var portConnections = Connections.ToList();

        foreach (var connection in portConnections)
        {
            RemoveConnection(connection);
            connection.Break();
        }
    }
}