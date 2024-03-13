using LogicSimulator.Core.Gates;

namespace LogicSimulator.Core;

public class GateFactory
{
    private readonly Simulator _simulator;

    public GateFactory(Simulator simulator) => _simulator = simulator;

    public AndGate CreateAndGate() => new(_simulator);

    public NandGate CreateNandGate() => new(_simulator);
    
    public InputGate CreateInputGate() => new(_simulator);
    
    public OutputGate CreateOutputGate() => new();
}