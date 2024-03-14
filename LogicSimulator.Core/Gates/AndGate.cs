using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class AndGate : SimpleGate
{
    public AndGate(Simulator simulator, int inputPortsCount) : base(simulator, inputPortsCount) { }

    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsAnd(a, b);
}