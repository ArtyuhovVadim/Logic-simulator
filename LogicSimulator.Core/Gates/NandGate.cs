using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class NandGate : SimpleGate
{
    public NandGate(Simulator simulator, int inputPortsCount) : base(simulator, inputPortsCount) { }

    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsNand(a, b);
}