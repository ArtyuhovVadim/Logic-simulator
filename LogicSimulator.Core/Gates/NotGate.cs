using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class NotGate : BaseGate
{
    public NotGate()
    {
        Output = new OutputPort(this);
        Input = new InputPort(this);
    }

    public override IEnumerable<BasePort> Ports => [Input, Output];

    public InputPort Input { get; }

    public OutputPort Output { get; }

    public ulong Delay { get; set; }

    protected sealed override void OnInvalidate(Simulator simulator)
    {
        var newState = SignalsCalculator.CalculateAsNot(Input.State);

        if (Output.State == newState)
            return;

        simulator.PushEvent(Output, newState, Delay);
    }
}