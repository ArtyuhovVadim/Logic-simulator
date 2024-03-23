namespace LogicSimulator.Core;

public class Connection
{
    private OutputPort? _sourcePort;
    private readonly List<InputPort> _receiverPorts;

    public Connection(OutputPort sourcePort, InputPort receiverPort) : this(sourcePort, [receiverPort]) { }

    public Connection(OutputPort sourcePort, params InputPort[] receiverPorts) : this(sourcePort, (IEnumerable<InputPort>)receiverPorts) { }

    public Connection(OutputPort sourcePort, IEnumerable<InputPort> receiverPorts)
    {
        if (!receiverPorts.Any())
            throw new InvalidOperationException("Receiver ports are empty.");

        _sourcePort = sourcePort;
        _receiverPorts = receiverPorts.ToList();

        _sourcePort.AddConnection(this);
        foreach (var receiverPort in _receiverPorts)
            receiverPort.AddConnection(this);
    }

    public void Invalidate(Simulator simulator)
    {
        if (_sourcePort is null)
            throw new InvalidOperationException("Source port is null");

        foreach (var port in _receiverPorts)
        {
            port.Invalidate(simulator, _sourcePort.State);
        }
    }

    public void RemoveReceiverPort(InputPort port) => _receiverPorts.Remove(port);

    public void Break()
    {
        foreach (var receiverPort in _receiverPorts)
            receiverPort.RemoveConnection(this);

        _sourcePort = null;
        _receiverPorts.Clear();
    }
}