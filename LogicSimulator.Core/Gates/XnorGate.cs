using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core.Gates;

public class XnorGate : SimpleGate
{
    public XnorGate(Simulator simulator, int inputPortsCount) : base(simulator, inputPortsCount) { }

    protected override SignalType GetOutputFromInput(SignalType a, SignalType b) => SignalsCalculator.CalculateAsXnor(a, b);
}