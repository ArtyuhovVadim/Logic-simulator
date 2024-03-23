using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class NorGate : SimpleGate
{
    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsNor(a, b);
}