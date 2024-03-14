using LogicSimulator.Core.Gates;

namespace LogicSimulator.Core;

public class GateFactory
{
    private readonly Simulator _simulator;

    public GateFactory(Simulator simulator) => _simulator = simulator;

    public InputGate CreateInputGate() => new(_simulator);

    public OutputGate CreateOutputGate() => new();

    public AndGate CreateAndGate(int inputsCount = 2) => new(_simulator, inputsCount);

    public NandGate CreateNandGate(int inputsCount = 2) => new(_simulator, inputsCount);

    public OrGate CreateOrGate(int inputsCount = 2) => new(_simulator, inputsCount);

    public NorGate CreateNorGate(int inputsCount = 2) => new(_simulator, inputsCount);

    public XorGate CreateXorGate(int inputsCount = 2) => new(_simulator, inputsCount);

    public XnorGate CreateXnorGate(int inputsCount = 2) => new(_simulator, inputsCount);

    public NotGate CreateNotGate() => new(_simulator);
}