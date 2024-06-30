using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class NandGate : SimpleGate
{
    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsNand(a, b);
}