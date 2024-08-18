using LogicSimulator.Core;
using LogicSimulator.Models.Common;
using LogicSimulator.Models.Logic;

namespace LogicSimulator.Models.SchemeGraph.Base;

public interface ISchemeGraphPortNode
{
    ISchemeGraphGateNode Parent { get; }

    PortType Type { get; }

    PortModel PortModel { get; }

    BasePort LogicModel { get; }

    IEnumerable<ISchemeGraphEdge> Connections { get; }
}