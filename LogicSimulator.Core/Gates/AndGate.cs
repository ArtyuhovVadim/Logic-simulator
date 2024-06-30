using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class AndGate : SimpleGate
{
    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsAnd(a, b);
}