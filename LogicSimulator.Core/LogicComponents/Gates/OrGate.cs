using System.Diagnostics;
using LogicSimulator.Core.LogicComponents.Gates.Base;

namespace LogicSimulator.Core.LogicComponents.Gates;

[DebuggerDisplay("Output {OutputPort}")]
public class OrGate : SingleOutputGate
{
    public OrGate(int inputPortsCount) : base(inputPortsCount)
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
            if (InputPorts[i].State == PortState.True)
            {
                OutputPort.State = PortState.True;
                return;
            }
        }

        OutputPort.State = PortState.False;
    }
}