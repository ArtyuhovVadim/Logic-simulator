using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class AndGate : BaseGate
{
    private readonly Simulator _simulator;

    public AndGate(Simulator simulator)
    {
        _simulator = simulator;
        InputA = new Port(this, PortType.Input);
        InputB = new Port(this, PortType.Input);
        Output = new Port(this, PortType.Output);
    }

    public Port InputA { get; }

    public Port InputB { get; }

    public Port Output { get; }

    public long Delay { get; set; } = 0;

    protected override void OnInvalidate()
    {
        var newState = SignalsCalculator.CalculateAsAnd(InputA.State, InputB.State);
        _simulator.PushEvent(Output, newState, Delay);
    }
}