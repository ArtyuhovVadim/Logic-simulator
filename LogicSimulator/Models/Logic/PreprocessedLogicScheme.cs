using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Models.SchemeGraph;
using LogicSimulator.Models.SchemeGraph.Base;

namespace LogicSimulator.Models.Logic;

public class PreprocessedLogicScheme : IPreprocessedLogicScheme
{
    private readonly List<BaseGateModel> _gates;
    private readonly Dictionary<SchemeGraphGateNode, HashSet<SchemeGraphGateNode>> _graph;
    private readonly List<InvalidConnection> _invalidConnection;
    private readonly List<SchemeGraphEdge> _edges;
    private readonly List<WiresGroup> _wiresGroups;

    public PreprocessedLogicScheme(
        List<BaseGateModel> gates,
        Dictionary<SchemeGraphGateNode, HashSet<SchemeGraphGateNode>> graph,
        List<InvalidConnection> invalidConnection,
        List<SchemeGraphEdge> edges,
        List<WiresGroup> wiresGroups)
    {
        _gates = gates;
        _graph = graph;
        _invalidConnection = invalidConnection;
        _edges = edges;
        _wiresGroups = wiresGroups;
    }

    public IEnumerable<ISchemeGraphGateNode> Nodes => _graph.Keys;

    public IReadOnlyList<ISchemeGraphEdge> Edges => _edges;

    public IReadOnlyList<BaseGateModel> Gates => _gates;

    public IReadOnlyList<InvalidConnection> InvalidConnection => _invalidConnection;

    public IReadOnlyList<IWiresGroup> WiresGroups => _wiresGroups;
}