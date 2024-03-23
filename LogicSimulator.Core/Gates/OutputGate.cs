using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class OutputGate : BaseGate
{
    public OutputGate() => Input = new InputPort(this);

    public override IEnumerable<BasePort> Ports => [Input];

    public InputPort Input { get; set; }

    protected override void OnInvalidate(Simulator simulator) { }
}