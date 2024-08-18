using LogicSimulator.Models.Logic;
using LogicSimulator.Models.SchemeGraph.Base;
using SharpDX;

namespace LogicSimulator.Models.SchemeGraph;

public class WiresGroup : IWiresGroup
{
    public HashSet<WireModel> Wires { get; } = [];

    IReadOnlySet<WireModel> IWiresGroup.Wires => Wires;

    public bool ContainsPoint(Vector2 p) => Wires.Any(wire => wire.Segments.Any(x => x.ContainsPoint(p)));
}