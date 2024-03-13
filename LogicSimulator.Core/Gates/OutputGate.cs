using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class OutputGate : BaseGate
{
    public OutputGate() => Input = new Port(this, PortType.Input);

    public Port Input { get; set; }

    protected override void OnInvalidate() { }
}