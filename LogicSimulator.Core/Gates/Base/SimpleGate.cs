namespace LogicSimulator.Core.Gates.Base;

public abstract class SimpleGate : BaseGate
{
    protected SimpleGate(Simulator simulator, int inputPortsCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(inputPortsCount, 2);

        Simulator = simulator;
        Output = new Port(this, PortType.Output);

        var inputPorts = new List<Port>(inputPortsCount);

        for (var i = 0; i < inputPortsCount; i++)
            inputPorts.Add(new Port(this, PortType.Input));

        Inputs = inputPorts;
    }

    public IReadOnlyList<Port> Inputs { get; }

    public Port Output { get; }

    public long Delay { get; set; }

    protected Simulator Simulator { get; }

    protected sealed override void OnInvalidate()
    {
        var newState = Inputs[0].State;

        for (var i = 1; i < Inputs.Count; i++)
            newState = GetOutputFromInput(newState, Inputs[i].State);

        if (Output.State == newState)
            return;

        Simulator.PushEvent(Output, newState, Delay);
    }

    protected abstract SignalType GetOutputFromInput(SignalType a, SignalType b);
}