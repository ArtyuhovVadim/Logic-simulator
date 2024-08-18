using LogicSimulator.Models.Logic;
using SharpDX;

namespace LogicSimulator.Models.SchemeGraph.Base;

public interface IWiresGroup
{
    IReadOnlySet<WireModel> Wires { get; }

    bool ContainsPoint(Vector2 p);
}