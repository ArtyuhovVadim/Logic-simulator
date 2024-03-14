namespace LogicSimulator.Core;

public class Connection
{
    private readonly Port _sourcePort;
    private readonly List<Port> _receiverPorts;

    public Connection(Port sourcePort, Port receiverPort) : this(sourcePort, [receiverPort]) { }

    public Connection(Port sourcePort, params Port[] receiverPort) : this(sourcePort, (IEnumerable<Port>)receiverPort) { }

    public Connection(Port sourcePort, IEnumerable<Port> receiverPorts)
    {
        if (sourcePort.Type != PortType.Output)
            throw new InvalidOperationException("Source port must be output type.");

        var receiverPortsList = receiverPorts.ToList();

        if (receiverPortsList.Any(port => port.Type != PortType.Input))
            throw new InvalidOperationException("Receiver ports must be input type.");

        _sourcePort = sourcePort;
        _receiverPorts = receiverPortsList;

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