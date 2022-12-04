using System.Diagnostics;
using LogicSimulator.Core.LogicComponents;
using LogicSimulator.Core.LogicComponents.Base;
using LogicSimulator.Core.LogicComponents.Gates.Base;

namespace LogicSimulator.Core;

[DebuggerDisplay("Type: {Type}, State: {State}, ConnectedWiresCount: {ConnectedWiresCount}")]
public class Port : LogicComponent
{
    private readonly List<Wire> _wires = new();

    public int ConnectedWiresCount => _wires.Count;

    public BaseGate Owner { get; }

    public PortType Type { get; }

    public LogicState State { get; set; }

    public Port(BaseGate owner, PortType type)
    {
        Owner = owner;
        Type = type;
    }

    public void AddWire(Wire wire)
    {
        //TODO: Проверки
        _wires.Add(wire);
    }

    protected override void OnUpdate()
    {
        if (Type == PortType.Output)
        {
            for (var i = 0; i < ConnectedWiresCount; i++)
            {
                _wires[i].Update();
            }
        }
        else if (Type == PortType.Input)
        {
            Owner.IsDirty = true;
        }
    }
}