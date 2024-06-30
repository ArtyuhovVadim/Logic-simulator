using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class XnorGate : SimpleGate
{
    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsXnor(a, b);
}