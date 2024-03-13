namespace LogicSimulator.Core;

public class Connection
{
    private readonly Port _sourcePort;
    private readonly IEnumerable<Port> _receiverPorts;

    public Connection(Port sourcePort, Port receiverPort) : this(sourcePort, [receiverPort]) { }

    public Connection(Port sourcePort, params Port[] receiverPort) : this(sourcePort, (IEnumerable<Port>)receiverPort) { }

    public Connection(Port sourcePort, IEnumerable<Port> receiverPorts)
    {
        if (sourcePort.Type != PortType.Output)
            throw new InvalidOperationException("Source port must be output type.");

        if (receiverPorts.Any(port => port.Type != PortType.Input))
            throw new InvalidOperationException("Receiver ports must be input type.");

        _sourcePort = sourcePort;
        _receiverPorts = receiverPorts;

        sourcePort.AddConnection(this);
    }

    public void Invalidate()
    {
        foreach (var port in _receiverPorts)
        {
            port.State = _sourcePort.State;
        }
    }
}