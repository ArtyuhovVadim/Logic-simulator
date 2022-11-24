using System.Diagnostics;
using LogicSimulator.Core.LogicComponents.Gates.Base;

namespace LogicSimulator.Core.LogicComponents.Gates;

[DebuggerDisplay("Output {OutputPort}")]
public class AndGate : SingleOutputGate
{
    public AndGate(int inputPortsCount) : base(inputPortsCount)
    {
        if (inputPortsCount < 2)
            throw new ArgumentOutOfRangeException(nameof(inputPortsCount));
    }

    public override void Update()
    {
        if (InputPorts.Any(t => t.State == PortState.Undefined))
        {
            OutputPort.State = PortState.Undefined;
            return;
        }

        for (var i = 0; i < InputPortsCount; i++)
        {
            if (InputPorts[i].State == PortState.False)
            {
                OutputPort.State = PortState.False;
                return;
            }
        }

        OutputPort.State = PortState.True;
    }
}