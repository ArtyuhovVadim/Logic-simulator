using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class InputGate : BaseGate
{
    public InputGate()
    {
        Output = new OutputPort(this);
    }

    public override IEnumerable<BasePort> Ports => [Output];

    public SignalType State { get; set; } = SignalType.Undefined;

    public OutputPort Output { get; set; }

    public ulong Delay { get; set; }

    protected override void OnInvalidate(Simulator simulator)
    {
        simulator.PushEvent(Output, State, Delay);
    }
}