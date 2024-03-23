using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core;

public class InputPort : BasePort
{
    public InputPort(BaseGate parent, SignalType initialState = SignalType.Undefined) : base(parent, initialState) { }

    public override void AddConnection(Connection connection)
    {
        if (ConnectionsInternal.Count != 0)
            throw new InvalidOperationException("Input port can be connected to one connection.");

        ConnectionsInternal.Add(connection);
    }

    public override void Invalidate(Simulator simulator, SignalType newState)
    {
        if (State == newState)
            return;

        State = newState;

        Parent.Invalidate(simulator);
    }

    public override void RemoveAllConnections()
    {
        var portConnections = Connections.ToList();

        foreach (var connection in portConnections)
        {
            RemoveConnection(connection);
            connection.RemoveReceiverPort(this);
        }
    }
}