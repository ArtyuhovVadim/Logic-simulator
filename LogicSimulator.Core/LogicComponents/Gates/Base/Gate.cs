﻿using LogicSimulator.Core.LogicComponents.Base;

namespace LogicSimulator.Core.LogicComponents.Gates.Base;

public abstract class Gate : LogicComponent
{
    public event Action Updated;

    protected List<Port> Ports { get; private set; } = new();

    protected Gate(int inputCount, int outputCount)
    {
        if (inputCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(inputCount));

        if (outputCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(inputCount));

        for (var i = 0; i < inputCount; i++)
        {
            Ports.Add(new Port(this, PortType.Input));
        }

        for (var i = 0; i < outputCount; i++)
        {
            Ports.Add(new Port(this, PortType.Output));
        }
    }

    public Port GetPort(int index)
    {
        return Ports[index];
    }
}