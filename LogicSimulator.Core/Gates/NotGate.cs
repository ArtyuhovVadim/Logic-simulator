using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class NotGate : BaseGate
{
    public NotGate(Simulator simulator)
    {
        Simulator = simulator;
        Output = new Port(this, PortType.Output);
        Input = new Port(this, PortType.Input);
    }

    public Port Input { get; }

    public Port Output { get; }

    public long Delay { get; set; }

    protected Simulator Simulator { get; }

    protected sealed override void OnInvalidate()
    {
        var newState = SignalsCalculator.CalculateAsNot(Input.State);

        if (Output.State == newState)
            return;

        Simulator.PushEvent(Output, newState, Delay);
    }
}