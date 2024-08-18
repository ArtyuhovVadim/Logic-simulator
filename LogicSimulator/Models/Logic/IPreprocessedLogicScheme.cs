using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Models.SchemeGraph;
using LogicSimulator.Models.SchemeGraph.Base;

namespace LogicSimulator.Models.Logic;

public interface IPreprocessedLogicScheme
{
    IReadOnlyList<BaseGateModel> Gates { get; }

    IEnumerable<ISchemeGraphGateNode> Nodes { get; }

    IReadOnlyList<ISchemeGraphEdge> Edges { get; }

    IReadOnlyList<IWiresGroup> WiresGroups { get; }

    IReadOnlyList<InvalidConnection> InvalidConnection { get; }
}