namespace LogicSimulator.Core.LogicComponents.Gates.Base;

public abstract class SingleOutputGate : Gate
{
    protected Port OutputPort { get; }

    protected SingleOutputGate(int inputPortsCount) : base(inputPortsCount)
    {
        OutputPort = new Port(this, PortType.Output);
    }

    public Port GetInputPort(int i) => InputPorts[i];

    public Port GetOutputPort() => OutputPort;
}