using System.Diagnostics;
using LogicSimulator.Core.LogicComponents.Gates.Base;

namespace LogicSimulator.Core;

[DebuggerDisplay("State: {State}")]
public class Port
{
    private readonly Gate _owner;
    private readonly PortType _type;

    public PortState State { get; set; }

    public Port(Gate owner, PortType type)
    {
        _owner = owner;
        _type = type;
    }
}