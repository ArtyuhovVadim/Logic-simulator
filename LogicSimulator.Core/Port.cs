using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core;

public class Port
{
    private readonly BaseGate _parent;
    private SignalType _state;
    private readonly List<Connection> _connections = [];

    public Port(BaseGate parent, PortType type, SignalType initialState = SignalType.Undefined)
    {
        _parent = parent;
        _state = initialState;
        Type = type;
    }

    public PortType Type { get; }

    public SignalType State
    {
        get => _state;
        set
        {
            if (_state == value)
                return;

            _state = value;

            if (Type == PortType.Input)
            {
                _parent.Invalidate();
            }
            else
            {
                foreach (var connection in _connections)
                {
                    connection.Invalidate();
                }
            }
        }
    }

    public void AddConnection(Connection connection) => _connections.Add(connection);
}