using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class OrGate : SimpleGate
{
    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsOr(a, b);
}