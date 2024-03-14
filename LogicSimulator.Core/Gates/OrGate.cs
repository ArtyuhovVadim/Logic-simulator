using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class OrGate : SimpleGate
{
    public OrGate(Simulator simulator, int inputPortsCount) : base(simulator, inputPortsCount) { }

    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsOr(a, b);
}