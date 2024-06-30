using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class XorGate : SimpleGate
{
    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsXor(a, b);
}