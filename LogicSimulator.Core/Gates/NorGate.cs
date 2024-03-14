using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class NorGate : SimpleGate
{
    public NorGate(Simulator simulator, int inputPortsCount) : base(simulator, inputPortsCount) { }

    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsNor(a, b);
}