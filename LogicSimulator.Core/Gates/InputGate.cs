using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class InputGate : BaseGate
{
    private readonly Simulator _simulator;

    public InputGate(Simulator simulator)
    {
        _simulator = simulator;
        Output = new Port(this, PortType.Output);
    }

    public SignalType State { get; set; } = SignalType.Undefined;

    public Port Output { get; set; }

    public long Delay { get; set; } = 0;

    protected override void OnInvalidate()
    {
        _simulator.PushEvent(Output, State, Delay);
    }
}