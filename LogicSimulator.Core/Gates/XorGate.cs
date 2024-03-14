using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class XorGate : SimpleGate
{
    public XorGate(Simulator simulator, int inputPortsCount) : base(simulator, inputPortsCount) { }

    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsXor(a, b);
}