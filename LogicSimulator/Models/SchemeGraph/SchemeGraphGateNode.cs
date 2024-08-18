using System.Diagnostics;
using LogicSimulator.Core.Gates.Base;
using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Models.SchemeGraph.Base;

namespace LogicSimulator.Models.SchemeGraph;

[DebuggerDisplay("{GateModel.Name}")]
public class SchemeGraphGateNode : ISchemeGraphGateNode
{
    public SchemeGraphGateNode(BaseGateModel model)
    {
        GateModel = model;
        InputPortNodes = model.InputPorts.Select(x => new SchemeGraphPortNode(this, x)).ToHashSet();
        OutputPortNodes = model.OutputPorts.Select(x => new SchemeGraphPortNode(this, x)).ToHashSet();
    }

    public BaseGateModel GateModel { get; }

    public BaseGate LogicModel { get; set; } = null!;

    public IReadOnlySet<SchemeGraphPortNode> InputPortNodes { get; }

    public IReadOnlySet<SchemeGraphPortNode> OutputPortNodes { get; }

    public IEnumerable<SchemeGraphPortNode> PortNodes => InputPortNodes.Concat(OutputPortNodes);

    IEnumerable<ISchemeGraphPortNode> ISchemeGraphGateNode.InputPortNodes => InputPortNodes;

    IEnumerable<ISchemeGraphPortNode> ISchemeGraphGateNode.OutputPortNodes => OutputPortNodes;

    IEnumerable<ISchemeGraphPortNode> ISchemeGraphGateNode.PortNodes => InputPortNodes.Concat(OutputPortNodes);
}