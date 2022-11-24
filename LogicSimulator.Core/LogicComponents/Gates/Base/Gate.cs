using LogicSimulator.Core.LogicComponents.Base;

namespace LogicSimulator.Core.LogicComponents.Gates.Base;

public abstract class Gate : LogicComponent
{
    protected Port[] InputPorts { get; }

    public int InputPortsCount => InputPorts.Length;

    protected Gate(int inputPortsCount)
    {
        if (inputPortsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(inputPortsCount));

        InputPorts = new Port[inputPortsCount];

        for (var i = 0; i < inputPortsCount; i++)
        {
            InputPorts[i] = new Port(this, PortType.Input);
        }
    }

    public override void SetInputPortState(int i, PortState state)
    {
        if (i >= InputPortsCount || i < 0)
            throw new ArgumentOutOfRangeException(nameof(i));

        InputPorts[i].State = state;
    }
}